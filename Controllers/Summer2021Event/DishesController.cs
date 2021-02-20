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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

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
        public ActionResult<DishResponse> GetDish(Guid dishId)
        {
            var dish = _context
                .Dishes
                .Include(d => d.DishSongs)
                .FirstOrDefault(d => d.Id == dishId);
            if (dish == null)
            {
                return NotFound();
            }

            var gradedDishes = _context
                .GradedDishes
                .AsQueryable()
                .Where(g => g.DishId == dish.Id)
                .ToArray();
            return DishResponse.FromEntity(dish, gradedDishes);
        }

        [HttpPost]
        [Route("{dishId}/submission")]
        public async Task<ActionResult<GradedDancerDishResponse>> PostDishSubmission(
            [FromRoute] Guid dishId,
            [FromForm] GradedDancerDishRequest gradedDancerDishRequest)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            var dish = _context.Dishes.Include(d => d.DishSongs).AsQueryable().SingleOrDefault(d => d.Id == dishId);
            var ingredients = _context
                .DishIngredients
                .AsQueryable()
                .Where(i => i.DishId == dish.Id)
                .ToList();
            
            var gradedIngredients = _context
                .GradedDancerIngredients
                .Include(s => s.Score)
                .Include(i => i.GradedIngredient)
                .Where(g => ingredients.Exists(i => i.DishId == dish.Id))
                .GroupBy(ingredient => ingredient.Score.SongId)
                .Select(i => i
                    .OrderByDescending(i => i.Score.Value)
                    .First())
                .ToList();

            if (ingredients.Count != gradedIngredients.Count)
            {
                return BadRequest();
            }

            var scores = new List<Score>();
            foreach (var scoreRequest in gradedDancerDishRequest.Scores)
            {
                var score = await _context
                    .Scores
                    .AddAsync(new Score()
                    {
                        Value = scoreRequest.Score,
                        SongId = scoreRequest.SongId,
                        DancerId = existingDancer.Id
                    });
                scores.Add(score.Entity);
                try
                {
                    await using var memoryStream = scoreRequest.ScoreImage.OpenReadStream();

                    var destinationKey = $"Songs/{score.Entity.SongId}/Scores/{score.Entity.Id}.png";
                    await _fileStorage.UploadFileFromStream(memoryStream, destinationKey);
                }
                catch
                {
                    return BadRequest();
                }
            }

            var ingredientStars = gradedIngredients
                .Aggregate(0, (acc, g) => acc + (int) g.GradedIngredient.Grade) / 2;
            var exPercent = 100.0;
            var ex = Math.Pow(0.00573 * Math.E, 5.73 * exPercent);
            var ordering = scores.Aggregate(0, (acc, s) =>
            {
                var dishOrder = dish.DishSongs.FirstOrDefault(ds => ds.SongId == s.Id);
                if (dishOrder == null) return 0;
                return 0;
            });
            
            //await _context.SaveChangesAsync();
            return Ok();
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