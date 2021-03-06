using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
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
    public class IngredientsController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IDancer _dancerService;
        private readonly IGradedDancerIngredient _gradedDancerIngredientService;
        private readonly IGradedIngredient _gradedIngredientService;
        private readonly IIngredient _ingredientService;
        private readonly IScore _scoreService;
        private readonly IFileStorage _fileStorage;

        public IngredientsController(ILogger<IngredientsController> logger,
            ICoreData coreDataService,
            IDancer dancerService,
            IGradedDancerIngredient gradedDancerIngredientService,
            IGradedIngredient gradedIngredientService,
            IIngredient ingredientService,
            IScore scoreService,
            IFileStorage fileStorage)
        {
            _logger = logger;
            _coreDataService = coreDataService;
            _dancerService = dancerService;
            _gradedDancerIngredientService = gradedDancerIngredientService;
            _gradedIngredientService = gradedIngredientService;
            _ingredientService = ingredientService;
            _scoreService = scoreService;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IngredientResponse>> Get()
        {
            return _ingredientService
                .GetAll()
                .Select(IngredientResponse.FromEntity)
                .ToArray();
        }

        [HttpGet]
        [Route("~/summer2021/dancers/{dancerId}/ingredients")]
        public ActionResult<IEnumerable<GradedDancerIngredientResponse>> GetGradedIngredientsForDancer(
            Guid dancerId, 
            [FromQuery(Name = "top_only")] bool topOnly = true)
        {
            var gradedIngredients = topOnly ? 
                _gradedDancerIngredientService.GetTopForDancer(dancerId) :
                _gradedDancerIngredientService.GetAllForDancer(dancerId);
            
            return Ok(gradedIngredients.Select(GradedDancerIngredientResponse.FromEntity));
        }

        [HttpGet]
        [Route("{ingredientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IngredientResponse> Get(Guid ingredientId)
        {
            var ingredient = _ingredientService.Get(ingredientId);
            if (ingredient == null)
            {
                return NotFound();
            }

            return IngredientResponse.FromEntity(ingredient);
        }

        [HttpGet]
        [Route("{ingredientId}/grades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<GradedIngredientResponse>> GetGrades(Guid ingredientId)
        {
            var ingredient = _ingredientService.Get(ingredientId);
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(_gradedIngredientService.GetAllForIngredient(ingredient.Id)
                .Select(GradedIngredientResponse.FromEntity));
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IngredientResponse>> Post([FromForm] IngredientRequest ingredientRequest)
        {
            var ingredient = ingredientRequest.ToEntity();
            var newIngredient = await _ingredientService.Add(ingredient);

            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                var ingredientImage = await Image.LoadAsync(ingredientRequest.IngredientImage!.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(ingredientImage, size, size);

                    var destinationKey = $"summer2021/ingredients/{newIngredient.Id}.{size}.png";
                    await _fileStorage.UploadFileFromStream(image, destinationKey);
                }
            }
            catch
            {
                return BadRequest();
            }

            await _coreDataService.SaveChanges();
            return Ok(IngredientResponse.FromEntity(newIngredient));
        }
        
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{ingredientId}")]
        public async Task<ActionResult<GradedDancerIngredientResponse>> SubmitScoreForIngredient(
            [FromRoute] Guid ingredientId,
            [FromForm] IngredientScoreRequest request)
        {
            var authId = HttpContext.GetUserId();
            var existingDancer = _dancerService.GetByAuthId(authId) ?? await _dancerService.Add(new Dancer{AuthenticationId = authId});
            if (existingDancer == null)
            {
                return NotFound("dancer does not exist");
            }

            var ingredient = _ingredientService.Get(ingredientId);
            if (ingredient == null) return BadRequest("ingredient does not exist");

            var gradedIngredient = _gradedIngredientService.GetForScore(ingredient.Id, request.Score);
            if (gradedIngredient?.Ingredient == null)
            {
                return NotFound($"graded ingredient does not exist for {ingredientId} score {request.Score}");
            }

            var score = await _scoreService.Add(new Score()
            {
                SongId = ingredient.SongId,
                DancerId = existingDancer.Id,
                Value = request.Score,
            });

            var dancerIngredient = await _gradedDancerIngredientService.Add(new GradedDancerIngredient()
            {
                GradedIngredientId = gradedIngredient.Id,
                DancerId = existingDancer.Id,
                ScoreId = score.Id,
            });

            try
            {
                var scoreImage = await Image.LoadAsync(request.ScoreImage!.OpenReadStream());
                var image = await Images.ImageToPngMemoryStreamFactor(scoreImage, 1000, 1000);

                var destinationKey = $"songs/{score.SongId}/scores/{score.Id}.png";
                await _fileStorage.UploadFileFromStream(image, destinationKey);
            }
            catch
            {
                return BadRequest("image was invalid or malformed");
            }

            await _coreDataService.SaveChanges();

            return GradedDancerIngredientResponse.FromEntity(dancerIngredient);
        }
    }
}