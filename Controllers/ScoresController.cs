using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using Microsoft.AspNetCore.Authorization;
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

        public ScoresController(ILogger<ScoresController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
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
        public async Task<ScoreResponse> SubmitScore(ScoreSubmissionRequest request)
        {
            var score = await _context.Scores.AddAsync(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = request.DancerId,
                ImageUrl = request.ImageUrl
            });
            await _context.SaveChangesAsync();
            return ScoreResponse.FromEntity(score.Entity);
        }
    }
}