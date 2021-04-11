using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.GradedDancerIngredient;
using AusDdrApi.Services.GradedIngredient;
using AusDdrApi.Services.Ingredient;
using AusDdrApi.Services.Score;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IDancerService _dancerService;
        private readonly IScore _scoreService;
        private readonly IIngredient _ingredientService;
        private readonly IGradedIngredient _gradedIngredientService;
        private readonly IGradedDancerIngredient _gradedDancerIngredientService;
        private readonly IFileStorage _fileStorage;

        public ScoresController(
            ILogger<ScoresController> logger,
            ICoreData coreDataService,
            IDancerService dancerService,
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
        [Route("~/summer2021/dancers/{dancerId}/scores")]
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
    }
}
