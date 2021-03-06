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
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.Score;
using AusDdrApi.Services.Song;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly ICoreData _coreService;
        private readonly IScore _scoreService;
        private readonly ISong _songService;
        private readonly IDancer _dancerService;
        private readonly IFileStorage _fileStorage;

        public ScoresController(
            ILogger<ScoresController> logger,
            ICoreData coreService,
            IScore scoreService,
            ISong songService,
            IDancer dancerService,
            IFileStorage fileStorage)
        {
            _logger = logger;
            _coreService = coreService;
            _scoreService = scoreService;
            _songService = songService;
            _dancerService = dancerService;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [Route("{scoreId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ScoreResponse> GetScore(Guid scoreId)
        {
            var score = _scoreService.Get(scoreId);
            if (score == null)
            {
                return NotFound();
            }
            return Ok(ScoreResponse.FromEntity(score));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ScoreResponse>> Get(
            [FromQuery(Name = "score_id")] Guid[]? scoreIds,
            [FromQuery(Name = "dancer_id")] Guid[]? dancerIds,
            [FromQuery(Name = "song_id")] Guid[]? songIds,
            [FromQuery(Name = "top_scores_only")] bool topScoresOnly = true
        )
        {
            if (dancerIds?.Length == 0 && songIds?.Length == 0)
            {
                return BadRequest();
            }
            return Ok(_scoreService.GetScores(dancerIds, songIds, topScoresOnly).Select(ScoreResponse.FromEntity));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{songId}")]
        public ActionResult<IEnumerable<ScoreResponse>> Get(
            Guid songId,
            bool topScoresOnly = true)
        {
            var scores = topScoresOnly
                ? _scoreService.GetTopScores(songId)
                : _scoreService.GetScores(null, new Guid[] {songId}, false);
            return Ok(scores.Select(ScoreResponse.FromEntity));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ScoreResponse>> SubmitScore([FromForm] ScoreSubmissionRequest request)
        {
            var authId = HttpContext.GetUserId();
            var dancer = _dancerService.GetByAuthId(authId) ?? await _dancerService.Add(new Dancer{AuthenticationId = authId});
            if (dancer == null)
            {
                return NotFound();
            }

            var song = _songService.Get(request.SongId);
            if (song == null)
            {
                return NotFound();
            }
            if (request.Score > song!.MaxScore) return BadRequest();

            var score = await _scoreService.Add(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = dancer.Id,
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
                return BadRequest();
            }
            
            await _coreService.SaveChanges();
            
            return ScoreResponse.FromEntity(score);
        }

        [HttpDelete]
        [Route("{scoreId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete([FromRoute] Guid scoreId)
        {
            if (!_scoreService.Delete(scoreId))
            {
                return NotFound();
            }
            await _coreService.SaveChanges();
            return Ok();
        }
    }
}
