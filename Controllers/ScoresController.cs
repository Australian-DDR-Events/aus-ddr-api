using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
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
        [Route("~/dancer/{dancerId}/score")]
        public IEnumerable<Score> GetScoresByDancerId(Guid dancerId)
        {
            return _context.Scores
                .Include(s => s.Song)
                .AsQueryable().Where(score => score.DancerId == dancerId);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<Score> SubmitScore(ScoreSubmissionRequest request)
        {
            var score = await _context.Scores.AddAsync(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = request.DancerId
            });
            await _context.SaveChangesAsync();
            return score.Entity;
        }
    }
}