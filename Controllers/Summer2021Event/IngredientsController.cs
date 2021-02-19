using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private DatabaseContext _context;
        private IFileStorage _fileStorage;

        public IngredientsController(ILogger<IngredientsController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IngredientResponse>> Get()
        {
            return _context
                .Ingredients
                .Include(s => s.Song)
                .Select(IngredientResponse.FromEntity)
                .ToArray();
        }

        [HttpGet]
        [Route("{ingredientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IngredientWithGradingResponse> Get(Guid ingredientId)
        {
            var ingredient = _context
                .Ingredients
                .FirstOrDefault(ingredient => ingredient.Id == ingredientId);
            if (ingredient == null)
            {
                return NotFound();
            }

            var gradedIngredients = _context
                .GradedIngredients
                .AsQueryable()
                .Where(gi => gi.IngredientId == ingredient.Id)
                .ToArray();

            return IngredientWithGradingResponse.FromEntity(ingredient, gradedIngredients);
        }
    }
}