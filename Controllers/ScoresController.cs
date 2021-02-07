using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

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
        [Route("~/dancers/{dancerId}/scores")]
        public IEnumerable<ScoreResponse> GetScoresByDancerId(Guid dancerId)
        {
            return _context.Scores
                .Include(s => s.Song)
                .AsQueryable().Where(score => score.DancerId == dancerId).AsEnumerable().Select(ScoreResponse.FromEntity);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ScoreResponse>> SubmitScore(ScoreSubmissionRequest request)
        {
            var score = await _context.Scores.AddAsync(new Score
            {
                Value = request.Score,
                SongId = request.SongId,
                DancerId = request.DancerId,
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