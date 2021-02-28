using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Entities;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.Dish;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.GradedDancerDish;
using AusDdrApi.Services.GradedDancerIngredient;
using AusDdrApi.Services.GradedDish;
using AusDdrApi.Services.GradedIngredient;
using AusDdrApi.Services.Ingredient;
using AusDdrApi.Services.Score;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IDancer _dancerService;
        private readonly IDish _dishService;
        private readonly IIngredient _ingredientService;
        private readonly IGradedDancerIngredient _gradedDancerIngredientService;
        private readonly IGradedDancerDish _gradedDancerDishService;
        private readonly IGradedDish _gradedDishService;
        private readonly IScore _scoreSerivce;
        private IFileStorage _fileStorage;

        public DishesController(
            ILogger<IngredientsController> logger,
            ICoreData coreDataService,
            IDancer dancerService,
            IDish dishService,
            IIngredient ingredientService,
            IGradedDancerIngredient gradedDancerIngredientService,
            IGradedDancerDish gradedDancerDishService,
            IGradedDish gradedDishService,
            IScore scoreService,
            IFileStorage fileStorage)
        {
            _logger = logger;
            _coreDataService = coreDataService;
            _dancerService = dancerService;
            _dishService = dishService;
            _ingredientService = ingredientService;
            _gradedDancerIngredientService = gradedDancerIngredientService;
            _gradedDancerDishService = gradedDancerDishService;
            _gradedDishService = gradedDishService;
            _scoreSerivce = scoreService;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DishResponse>> Get()
        {
            return Ok(_dishService.GetAll().Select(DishResponse.FromEntity).AsEnumerable());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{dishId}")]
        public ActionResult<DishResponse> Get(Guid dishId)
        {
            var dish = _dishService.Get(dishId);
            if (dish == null)
            {
                return NotFound();
            }
            return Ok(DishResponse.FromEntity(dish));
        }

        [HttpGet]
        [Route("{dishId}/ingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Ingredient>> GetIngredients(Guid dishId)
        {
            return Ok(_dishService.GetIngredientsForDish(dishId).AsEnumerable());
        }
        
        [HttpGet]
        [Route("{dishId}/grades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<GradedDishResponse>> GetGrades(Guid dishId)
        {
            var dish = _dishService.Get(dishId);
            if (dish == null) return NotFound();

            var dishGrades = _gradedDishService.GetAllForDish(dish.Id);
            return Ok(dishGrades.Select(GradedDishResponse.FromEntity));
        }

        [HttpPost]
        [Route("{dishId}/submission")]
        public async Task<ActionResult<GradedDancerDishResponse>> PostDishSubmission(
            [FromRoute] Guid dishId,
            [FromForm] GradedDancerDishRequest gradedDancerDishRequest)
        {
            var authId = HttpContext.GetUserId();
            var existingDancer = _dancerService.GetByAuthId(authId);
            if (existingDancer == null) return NotFound();

            var dish = _dishService.Get(dishId);
            if (dish == null) return NotFound();

            var ingredients = _dishService.GetIngredientsForDish(dish.Id);
            var gradedIngredients = _gradedDancerIngredientService.GetIngredientsForDancer(
                ingredients.Select(i => i.Id).ToList(),
                existingDancer.Id);
            if (ingredients.Count() != gradedIngredients.Count()) return BadRequest();
            
            var dishSongs = _dishService.GetSongsForDish(dish.Id);
            if (gradedDancerDishRequest.Scores.Count() != dishSongs.Count()) return BadRequest();

            var scores = new List<Score>();
            var uploadTasks = new List<Task<string>>();
            foreach (var scoreRequest in gradedDancerDishRequest.Scores)
            {
                var score = await _scoreSerivce.Add(new Score
                {
                    Value = scoreRequest.Score,
                    SongId = scoreRequest.SongId,
                    DancerId = existingDancer.Id
                });
                scores.Add(score);
                try
                {
                    var scoreImage = await Image.LoadAsync(scoreRequest.ScoreImage!.OpenReadStream());
                    var image = await Images.ImageToPngMemoryStream(scoreImage);

                    var destinationKey = $"songs/{score.SongId}/scores/{score.Id}.png";
                    uploadTasks.Add(_fileStorage.UploadFileFromStream(image, destinationKey));
                }
                catch
                {
                    return BadRequest();
                }
            }

            var grade = calculateGrade(gradedIngredients, scores, dish, dishSongs, gradedDancerDishRequest.PairBonus);

            var gradedDish = _gradedDishService
                .GetForDishIdAndGrade(dishId, grade);
            if (gradedDish == null) return NotFound();
            var gradedDancerDish = await _gradedDancerDishService
                .Add(new GradedDancerDish
                {
                    DancerId = existingDancer.Id,
                    GradedDishId = gradedDish.Id,
                    Scores = scores
                });

            if (gradedDancerDish == null) return BadRequest();
            
            try
            {
                var scoreImage = await Image.LoadAsync(gradedDancerDishRequest.FinalImage!.OpenReadStream());
                var image = await Images.ImageToPngMemoryStream(scoreImage);

                var destinationKey = $"dishes/{dish.Id}/final/{gradedDancerDish.Id}.png";
                uploadTasks.Add(_fileStorage.UploadFileFromStream(image, destinationKey));
            }
            catch
            {
                return BadRequest();
            }

            var t = Task.WhenAll(uploadTasks);
            try
            {
                t.Wait();
            }
            catch
            {
                return BadRequest();
            }

            await _coreDataService.SaveChanges();
            return Ok(GradedDancerDishResponse.FromEntity(gradedDancerDish, dish.Id));
        }

        private Grade calculateGrade(
            IEnumerable<GradedDancerIngredient> gradedIngredients,
            IList<Score> scores,
            Dish dish,
            IEnumerable<DishSong> dishSongs,
            bool pairBonus
            )
        {            
            var ingredientStars = gradedIngredients
                .Aggregate(0, (acc, g) => acc + (int) g.GradedIngredient!.Grade);
            var avgStars = (float) ingredientStars / gradedIngredients.Count();
            var exPercent = (double)scores.Aggregate(0, (count, s) => count += s.Value) / dish.MaxScore;
            var ex = 0.00573 * Math.Pow(Math.E, 5.73 * exPercent);

            var orderVariance = 0;
            var maxVariance = scores.Count - 1;
            for (var scoreIndex = 1; scoreIndex < scores.Count; ++scoreIndex)
            {
                var firstSong = dishSongs.FirstOrDefault(i => i.SongId == scores[scoreIndex - 1].SongId);
                var secondSong = dishSongs.FirstOrDefault(i => i.SongId == scores[scoreIndex].SongId);
                if (firstSong?.CookingOrder + 1 == secondSong?.CookingOrder) orderVariance++;
            }

            var varianceMultiplier = 1 + (orderVariance / maxVariance) * 0.5;

            var top = (avgStars + 1) / 2 + ex * varianceMultiplier;
            var baseGrade = Math.Floor((top / 1.1) * (pairBonus ? 1.1 : 1.0));
            return (Grade) (Math.Max(Math.Min(baseGrade, 4), 0));
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(DishRequest dishRequest)
        {
            var dish = await _dishService.Add(new Dish()
            {
                Name = dishRequest.Name
            });
            await _dishService.AddDishIngredients(
                dishRequest.IngredientIds.Select(
                    ingredientId => new DishIngredient()
                    {
                        DishId = dish.Id,
                        IngredientId = ingredientId
                    }
                )
            );
            await _dishService.AddDishSongs(
                dishRequest.SongIds.Select(
                    (songId, index) => new DishSong()
                    {
                        SongId = songId,
                        CookingOrder = index,
                        DishId = dish.Id
                    }
                )
            );

            for (var gradeIndex = 0; gradeIndex < dishRequest.GradeDescriptions.Count; gradeIndex++)
            {
                await _gradedDishService.Add(new GradedDish
                {
                    DishId = dish.Id,
                    Description = dishRequest.GradeDescriptions[gradeIndex],
                    Grade = (Grade)gradeIndex
                });
            }

            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                var dishImage = await Image.LoadAsync(dishRequest.DishImage!.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(dishImage, size, size);

                    var destinationKey = $"summer2021/dishes/{dish.Id}.{size}.png";
                    await _fileStorage.UploadFileFromStream(image, destinationKey);
                }
            }
            catch
            {
                return BadRequest();
            }
            
            await _coreDataService.SaveChanges();

            return Ok();
        }
        
        [HttpPost]
        [Route("{dishId}/adddishimage")]
        public async Task<ActionResult> PostDishImage([FromForm] IFormFile formImage, [FromRoute] string dishId)
        {
            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                var dishImage = await Image.LoadAsync(formImage.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(dishImage, size, size);

                    var destinationKey = $"summer2021/dishes/{dishId}.{size}.png";
                    await _fileStorage.UploadFileFromStream(image, destinationKey);
                }
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpPost]
        [Route("{dishId}/addgradeddishimage")]
        public async Task<ActionResult> PostGradedDishImage([FromForm] IFormFile formImage, [FromRoute] string dishId)
        {
            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                var dishImage = await Image.LoadAsync(formImage.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(dishImage, size, size);

                    var destinationKey = $"summer2021/gradeddishes/{dishId}.{size}.png";
                    await _fileStorage.UploadFileFromStream(image, destinationKey);
                }
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}