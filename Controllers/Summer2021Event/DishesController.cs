using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private DatabaseContext _context;
        private IFileStorage _fileStorage;

        public DishesController(ILogger<IngredientsController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DishResponse>> Get()
        {
            return _context.Dishes.Select(dish => DishResponse.FromEntity(dish, null)).ToArray();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{dishId}")]
        public ActionResult<IEnumerable<DishResponse>> GetDish(Guid dishId)
        {
            return _context.Dishes.Select(dish => DishResponse.FromEntity(dish, null)).ToArray();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(DishRequest dishRequest)
        {
            var dish = await _context.Dishes.AddAsync(new Dish()
            {
                Name = dishRequest.Name
            });
            await _context.DishIngredients.AddRangeAsync(
                dishRequest.IngredientIds.Select(
                    ingredientId => new DishIngredient()
                    {
                        DishId = dish.Entity.Id,
                        IngredientId = ingredientId
                    }
                )
            );
            await _context.DishSongs.AddRangeAsync(
                dishRequest.SongIds.Select(
                    (songId, index) => new DishSong()
                    {
                        SongId = songId,
                        CookingOrder = index,
                        DishId = dish.Entity.Id
                    }
                )
            );
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{dishId}")]
        public async Task<ActionResult> Post([FromRoute] Guid dishId, GradedDishRequest gradedDishRequest)
        {
            await _context.GradedDishes.AddAsync(new GradedDish()
            {
                DishId = dishId,
                Description = gradedDishRequest.Description,
                Grade = (Grade)gradedDishRequest.Grade
            });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}