using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Entities;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.GradedDancerIngredient;
using AusDdrApi.Services.GradedIngredient;
using AusDdrApi.Services.Ingredient;
using AusDdrApi.Services.Score;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IDancer _dancerService;
        private readonly IScore _scoreService;
        private readonly IIngredient _ingredientService;
        private readonly IGradedIngredient _gradedIngredientService;
        private readonly IGradedDancerIngredient _gradedDancerIngredientService;
        private readonly IFileStorage _fileStorage;

        public ScoresController(
            ILogger<ScoresController> logger,
            ICoreData coreDataService,
            IDancer dancerService,
            IScore scoreService,
            IIngredient ingredientService,
            IGradedIngredient gradedIngredientService,
            IGradedDancerIngredient gradedDancerIngredientService, 
            IFileStorage fileStorage)
        {
            _logger = logger;
            _coreDataService = coreDataService;
            _dancerService = dancerService;
            _scoreService = scoreService;
            _ingredientService = ingredientService;
            _gradedIngredientService = gradedIngredientService;
            _gradedDancerIngredientService = gradedDancerIngredientService;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GradedDancerIngredientResponse> Get(
            [FromQuery(Name = "dancer_id")] Guid dancerId,
            [FromQuery(Name = "ingredient_id")] Guid ingredientId)
        {
            var existingDancer = _dancerService.Get(dancerId);
            if (existingDancer == null)
            {
                return NotFound("dancer does not exist in this context");
            }

            var gradedDancerIngredient = _gradedDancerIngredientService.GetIngredientForDancer(ingredientId, dancerId);
            if (gradedDancerIngredient == null)
            {
                return NotFound($"dancer does not have ingredient {ingredientId}");
            }

            return Ok(GradedDancerIngredientResponse.FromEntity(gradedDancerIngredient));
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{dancerId}")]
        public ActionResult<IEnumerable<GradedDancerIngredientResponse>> GetIngredientsForDancer(
            Guid dancerId)
        {
            var existingDancer = _dancerService.Get(dancerId);
            if (existingDancer == null)
            {
                return NotFound("dancer does not exist in this context");
            }

            var gradedDancerIngredient = _gradedDancerIngredientService.GetTopForDancer(dancerId);
            
            return Ok(gradedDancerIngredient.Select(GradedDancerIngredientResponse.FromEntity));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GradedDancerIngredientResponse>> SubmitScoreForIngredient(
            GradedDancerIngredientSubmissionRequest request)
        {
            var authId = HttpContext.GetUserId();
            var existingDancer = _dancerService.GetByAuthId(authId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            var ingredient = _ingredientService.Get(request.IngredientId);
            if (ingredient == null || ingredient.SongId != request.ScoreRequest!.SongId) return BadRequest();

            var gradedIngredient = _gradedIngredientService.GetForScore(ingredient.Id, request.ScoreRequest.Score);
            if (gradedIngredient?.Ingredient == null)
            {
                return NotFound();
            }

            var score = await _scoreService.Add(new Score()
            {
                SongId = gradedIngredient.Ingredient.SongId,
                DancerId = existingDancer.Id,
                Value = request.ScoreRequest.Score,
            });

            var dancerIngredient = await _gradedDancerIngredientService.Add(new GradedDancerIngredient()
            {
                GradedIngredientId = gradedIngredient.Id,
                DancerId = existingDancer.Id,
                ScoreId = score.Id,
            });

            try
            {
                var scoreImage = await Image.LoadAsync(request.ScoreRequest.ScoreImage!.OpenReadStream());
                var image = await Images.ImageToPngMemoryStream(scoreImage);

                var destinationKey = $"songs/{score.SongId}/scores/{score.Id}.png";
                await _fileStorage.UploadFileFromStream(image, destinationKey);
            }
            catch
            {
                return BadRequest();
            }

            await _coreDataService.SaveChanges();

            return GradedDancerIngredientResponse.FromEntity(dancerIngredient);
        }
    }
}
