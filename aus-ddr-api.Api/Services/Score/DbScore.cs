using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using ScoreEntity = AusDdrApi.Entities.Score;

namespace AusDdrApi.Services.Score
{
    public class DbScore : IScore
    {
        private readonly DatabaseContext _context;

        public DbScore(DatabaseContext context)
        {
            _context = context;
        }
        
        public ScoreEntity? Get(Guid scoreId)
        {
            return _context.Scores.AsQueryable().SingleOrDefault(s => s.Id == scoreId);
        }

        public IEnumerable<ScoreEntity> GetScores(Guid?[] dancerIds, Guid?[] songIds)
        {
            return _context
                .Scores
                .AsQueryable()
                .Where(score => dancerIds.Length <= 0 || dancerIds.Contains(score.DancerId))
                .Where(score => songIds.Length <= 0 || songIds.Contains(score.SongId))
                .ToList();
        }

        public async Task<ScoreEntity> Add(ScoreEntity score)
        {
            score.SubmissionTime = DateTime.Now;
            var scoreEntity = await _context.Scores.AddAsync(score);
            return scoreEntity.Entity;
        }

        public bool Delete(Guid scoreId)
        {
            var score = Get(scoreId);
            if (score != null)
            {
                _context.Scores.Remove(score);
                return true;
            }

            return false;
        }
    }
}