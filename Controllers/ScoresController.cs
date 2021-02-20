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
using AusDdrApi.Services.Entities.CoreService;
using AusDdrApi.Services.Entities.DancerService;
using AusDdrApi.Services.Entities.ScoreService;
using AusDdrApi.Services.Entities.SongService;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly ICoreService _coreService;
        private readonly IScoreService _scoreService;
        private readonly ISongService _songService;
        private readonly IDancerService _dancerService;
        private readonly IFileStorage _fileStorage;

        public ScoresController(
            ILogger<ScoresController> logger,
            ICoreService coreService,
            IScoreService scoreService,
            ISongService songService,
            IDancerService dancerService,
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
            var score = _scoreService.GetScore(scoreId);
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
            [FromQuery(Name = "dancer_id")] Guid? dancerId,
            [FromQuery(Name = "song_id")] Guid? songId
        )
        {
            if (dancerId == null && songId == null)
            {
                return BadRequest();
            }
            return Ok(_scoreService.GetScores(dancerId, songId).Select(ScoreResponse.FromEntity));
        }
        
        [HttpPost]
        [Authorize]
        [Route("submit")]
        public async Task<ActionResult<ScoreResponse>> SubmitScore([FromForm] ScoreSubmissionRequest request)
        {
            var authId = HttpContext.GetUserId();
            var dancer = _dancerService.GetByAuthId(authId);
            if (dancer == null)
            {
                return NotFound();
            }

            var score = await _scoreService.Add(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = dancer.Id,
            });

            try
            {
                var image = await Images.FormFileToPngMemoryStream(request.ScoreImage);

                var destinationKey = $"Songs/{score.SongId}/Scores/{score.Id}.png";
                await _fileStorage.UploadFileFromStream(image, destinationKey);
            }
            catch
            {
                return BadRequest();
            }
            
            await _coreService.SaveChanges();
            
            return ScoreResponse.FromEntity(score);
        }
    }
}
