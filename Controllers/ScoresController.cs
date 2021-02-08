using System;
using System.Collections.Generic;
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

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private DatabaseContext _context;
        private IFileStorage _fileStorage;

        public ScoresController(ILogger<ScoresController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        [Route("{scoreId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ScoreResponse> GetScore(Guid scoreId)
        {
            var score = _context.Scores.AsQueryable().SingleOrDefault(score => score.Id == scoreId);
            if (score == null)
            {
                return NotFound();
            }

            return Ok(ScoreResponse.FromEntity(score));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ScoreResponse>> Get(
            [FromQuery(Name = "dancer_id")] Guid? dancerId,
            [FromQuery(Name = "song_id")] Guid? songId
        )
        {
            var scores = _context.Scores
                .AsQueryable()
                .Where(score => (dancerId ?? score.DancerId) == score.DancerId &&
                                (songId ?? score.SongId) == score.SongId)
                .AsEnumerable()
                .Select(ScoreResponse.FromEntity);

            return Ok(scores);
        }
 
        [HttpGet]
        [Route("~/dancers/{dancerId}/scores")]
        public IEnumerable<ScoreResponse> GetScoresByDancerId(Guid dancerId)
        {
            return _context.Scores
                .Include(s => s.Song)
                .AsQueryable().Where(score => score.DancerId == dancerId).AsEnumerable().Select(ScoreResponse.FromEntity);
        }
        
        [HttpPost]
        [Authorize]
        [Route("~/scores/submit")]
        public async Task<ActionResult<ScoreResponse>> SubmitScore(ScoreSubmissionRequest request)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }
            var score = await _context.Scores.AddAsync(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = existingDancer.Id,
            });

            try
            {
                await using var memoryStream = request.ScoreImage.OpenReadStream();

                var destinationKey = $"Songs/{score.Entity.SongId}/Scores/{score.Entity.Id}.png";
                await _fileStorage.UploadFileFromStream(memoryStream, destinationKey);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
            await _context.SaveChangesAsync();
            
            return ScoreResponse.FromEntity(score.Entity);
        }
    }
}