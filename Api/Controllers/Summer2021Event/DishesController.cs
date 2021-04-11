using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.Badges;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.Dish;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.GradedDancerDish;
using AusDdrApi.Services.GradedDancerIngredient;
using AusDdrApi.Services.GradedDish;
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
        private readonly IDancerService _dancerService;
        private readonly IDish _dishService;
        private readonly IIngredient _ingredientService;
        private readonly IGradedDancerIngredient _gradedDancerIngredientService;
        private readonly IGradedDancerDish _gradedDancerDishService;
        private readonly IGradedDish _gradedDishService;
        private readonly IScore _scoreSerivce;
        private readonly IBadge _badgeService;
        private IFileStorage _fileStorage;
        private IAuthorization _authorizationService;

        public DishesController(
            ILogger<IngredientsController> logger,
            ICoreData coreDataService,
            IDancerService dancerService,
            IDish dishService,
            IIngredient ingredientService,
            IGradedDancerIngredient gradedDancerIngredientService,
            IGradedDancerDish gradedDancerDishService,
            IGradedDish gradedDishService,
            IScore scoreService,
            IBadge badgeService,
            IFileStorage fileStorage,
            IAuthorization authorizationService)
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
            _badgeService = badgeService;
            _fileStorage = fileStorage;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DishResponse>> Get()
        {
            return Ok(_dishService.GetAll().Select(DishResponse.FromEntity).AsEnumerable());
        }

        [HttpGet]
        [Route("~/summer2021/dancers/{dancerId}/dishes")]
        public ActionResult<IEnumerable<GradedDancerDishResponse>> GetGradedDishesForDancer([FromRoute] Guid dancerId)
        {
            return _gradedDancerDishService
                .GetTopForDancer(dancerId)
                .Select(GradedDancerDishResponse.FromEntity)
                .ToList();
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
        [Route("{dishId}/songs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IngredientResponse>> GetDishSongs(Guid dishId)
        {
            return Ok( _dishService.GetSongsForDish(dishId).AsEnumerable().Select(DishSongResponse.FromEntity));
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
            var authId = _authorizationService.GetUserId();
            if (authId == null) return Unauthorized();
            var existingDancer = _dancerService.GetByAuthId(authId) ?? await _dancerService.Add(new Dancer{AuthenticationId = authId});
            if (existingDancer == null) return NotFound();

            var dish = _dishService.Get(dishId);
            if (dish == null) return NotFound();

            var gradedIngredients = _gradedDancerIngredientService.GetIngredientsForDancer(
                dish.Ingredients.Select(i => i.Id).ToList(),
                existingDancer.Id);
            if (dish.Ingredients.Count != gradedIngredients.Count()) return BadRequest();
            
            var dishSongs = _dishService.GetSongsForDish(dish.Id);
            
            if (gradedDancerDishRequest.Scores.Count() != dishSongs.Count()) return BadRequest();
            if (dishSongs.Any(dishSong => gradedDancerDishRequest.Scores.All(d => d.SongId != dishSong.SongId)))
            {
                return BadRequest();
            }
            
            foreach (var score in gradedDancerDishRequest.Scores)
            {
                var song = dishSongs.FirstOrDefault(s => s.SongId == score.SongId);
                if (song?.Song == null || song.Song.MaxScore < score.Score) return BadRequest();
            }

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
                    using var scoreImage = await Image.LoadAsync(scoreRequest.ScoreImage!.OpenReadStream());
                    var image = await Images.ImageToPngMemoryStreamFactor(scoreImage, 1000, 1000);

                    var destinationKey = $"songs/{score.SongId}/scores/{score.Id}.png";
                    uploadTasks.Add(_fileStorage.UploadFileFromStream(image, destinationKey));
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, $"Failed to add score image to file store: {e.Message}");
                    return BadRequest();
                }
            }

            var grade = CalculateGrade(gradedIngredients, scores, dish, dishSongs, gradedDancerDishRequest.PairBonus);

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
                using var scoreImage = await Image.LoadAsync(gradedDancerDishRequest.FinalImage!.OpenReadStream());
                var image = await Images.ImageToPngMemoryStreamFactor(scoreImage, 1000, 1000);

                var destinationKey = $"dishes/{dish.Id}/final/{gradedDancerDish.Id}.png";
                uploadTasks.Add(_fileStorage.UploadFileFromStream(image, destinationKey));
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to add score image to file store: {e.Message}");
                return BadRequest();
            }

            var t = Task.WhenAll(uploadTasks);
            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Failed to add score image to file store: {e.Message}");
                return BadRequest();
            }
            
            await _coreDataService.SaveChanges();
            
            AllocateBadge(existingDancer.Id);

            await _coreDataService.SaveChanges();
            gradedDancerDish = _gradedDancerDishService.Get(gradedDancerDish.Id);
            return Ok(GradedDancerDishResponse.FromEntity(gradedDancerDish!));
        }

        private Grade CalculateGrade(
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

            var varianceMultiplier = 1 + orderVariance / maxVariance * 0.5;

            var top = (avgStars + 1) / 2 + ex * varianceMultiplier;
            var baseGrade = Math.Floor(top / 1.1 * (pairBonus ? 1.1 : 1.0));
            return (Grade) Math.Max(Math.Min(baseGrade, 4), 0);
        }

        private void AllocateBadge(Guid dancerId)
        {
            var badgeThresholds = new []
            {
                (BadgeName: "Base", StarsRequired: 0),
                (BadgeName: "Red (I)", StarsRequired: 3),
                (BadgeName: "Red (II)", StarsRequired: 6),
                (BadgeName: "Blue (I)", StarsRequired: 8),
                (BadgeName: "Blue (II)", StarsRequired: 11),
                (BadgeName: "Green (I)", StarsRequired: 13),
                (BadgeName: "Green (II)", StarsRequired: 16),
                (BadgeName: "Gold (I)", StarsRequired: 18),
                (BadgeName: "Gold (II)", StarsRequired: 21),
                (BadgeName: "Opal", StarsRequired: 23),
            };
            var score = CalculateSeasonScore(dancerId);
            var eventId = Guid.Parse("94ed4523-5e6f-4534-89ba-574e813c5a33");
            var eventBadges = _badgeService.GetForEvent(eventId);

            var badgeName = badgeThresholds
                .OrderByDescending(b => b.StarsRequired)
                .First(b => b.StarsRequired <= score)
                .BadgeName;
            var newBadge = eventBadges.FirstOrDefault(b => b.Name == badgeName);
            if (newBadge == null) return;

            var badges = _badgeService.GetAssigned(dancerId);
            foreach (var assignedBadge in badges)
            {
                if (
                    badgeThresholds.Any(b => b.BadgeName == assignedBadge.Name) && 
                    assignedBadge.EventId == eventId
                )
                    _badgeService.RevokeBadge(assignedBadge.Id, dancerId);
            }

            _badgeService.AssignBadge(newBadge.Id, dancerId);
        }

        private int CalculateSeasonScore(Guid dancerId)
        {
            var dishes = _gradedDancerDishService.GetTopForDancer(dancerId).ToImmutableArray();
            // E starts 0 in the enum but when calculate, E should be equal to 1
            // The same applies to the other grades
            var score = dishes.Sum(d => (int) d.GradedDish!.Grade + 1); 
            return score;
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
            var ingredients = _ingredientService.Get(dishRequest.IngredientIds);
            _dishService.AddDishIngredients(dish.Id, ingredients);
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
                using var dishImage = await Image.LoadAsync(dishRequest.DishImage!.OpenReadStream());
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
    }
}