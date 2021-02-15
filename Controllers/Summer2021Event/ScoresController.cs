using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private DatabaseContext _context;
        private IFileStorage _fileStorage;

        public ScoresController(ILogger<ScoresController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [Authorize]
        [Route("~/summer2021/scores")]
        public async Task<ActionResult<GradedDancerIngredientResponse>> Get(
            [FromQuery(Name = "dancer_id")] Guid dancerId,
            [FromQuery(Name = "ingredient_id")] Guid ingredientId)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable()
                .SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            var gradedDancerIngredient = _context
                .GradedDancerIngredients
                .AsQueryable()
                .Where(ingredient =>
                    ingredient.DancerId == dancerId && ingredient.GradedIngredient.IngredientId == ingredientId)
                .OrderByDescending(ingredient => ingredient.Score.Value)
                .FirstOrDefault();
            if (gradedDancerIngredient == null)
            {
                return NotFound();
            }

            return GradedDancerIngredientResponse.FromEntity(gradedDancerIngredient);
        }

        [HttpPost]
        [Authorize]
        [Route("~/summer2021/scores/ingredient")]
        public async Task<ActionResult<GradedDancerIngredientResponse>> SubmitScoreForIngredient(
            GradedDancerIngredientSubmissionRequest request)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable()
                .SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            var gradedIngredient = _context
                .GradedIngredients
                .AsQueryable()
                .Where(ingredient => ingredient.RequiredScore <= request.Score)
                .OrderByDescending(ingredient => ingredient.RequiredScore)
                .FirstOrDefault();
            if (gradedIngredient == null)
            {
                return NotFound();
            }

            var score = await _context.Scores.AddAsync(new Score()
            {
                SongId = gradedIngredient.Ingredient.SongId,
                DancerId = existingDancer.Id,
                Value = request.Score,
            });

            var dancerIngredient = await _context.GradedDancerIngredients.AddAsync(new GradedDancerIngredient()
            {
                GradedIngredientId = gradedIngredient.Id,
                DancerId = existingDancer.Id,
                ScoreId = score.Entity.Id,
            });

            try
            {
                await using var memoryStream = request.ScoreImage.OpenReadStream();

                var destinationKey = $"Songs/{score.Entity.SongId}/Scores/{score.Entity.Id}.png";
                await _fileStorage.UploadFileFromStream(memoryStream, destinationKey);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return GradedDancerIngredientResponse.FromEntity(dancerIngredient.Entity);
        }
    }
}
