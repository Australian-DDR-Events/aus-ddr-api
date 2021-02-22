using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.GradedIngredient;
using AusDdrApi.Services.Ingredient;
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
        private readonly IGradedIngredient _gradedIngredientService;
        private readonly IIngredient _ingredientService;
        private readonly IFileStorage _fileStorage;

        public IngredientsController(ILogger<IngredientsController> logger,
            ICoreData coreDataService,
            IGradedIngredient gradedIngredientService,
            IIngredient ingredientService,
            IFileStorage fileStorage)
        {
            _logger = logger;
            _coreDataService = coreDataService;
            _gradedIngredientService = gradedIngredientService;
            _ingredientService = ingredientService;
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
    }
}