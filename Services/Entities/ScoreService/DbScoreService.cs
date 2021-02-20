using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.Services.Entities.ScoreService
{
    public class DbScoreService : IScoreService
    {
        private readonly DatabaseContext _context;

        public DbScoreService(DatabaseContext context)
        {
            _context = context;
        }
        
        public Score? GetScore(Guid scoreId)
        {
            return _context.Scores.AsQueryable().SingleOrDefault(s => s.Id == scoreId);
        }

        public IEnumerable<Score> GetScores(Guid? dancerId, Guid? songId)
        {
            return _context
                .Scores
                .AsQueryable()
                .Where(score => 
                    (dancerId ?? score.DancerId) == score.DancerId &&
                    (songId ?? score.SongId) == score.SongId)
                .AsEnumerable();
        }

        public async Task<Score> Add(Score score)
        {
            var scoreEntity = await _context.Scores.AddAsync(score);
            return scoreEntity.Entity;
        }
    }
}