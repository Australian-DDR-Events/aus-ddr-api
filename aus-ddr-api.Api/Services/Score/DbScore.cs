using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<ScoreEntity> GetTopScores(Guid[] songIds)
        {
            return _context
                .Scores
                .AsQueryable()
                .Include(s => s.Dancer)
                .Include(s => s.Song)
                .AsEnumerable()
                .GroupBy(x => x.SongId)
                .Select(x => x.Aggregate(
                    (l, r) => l.Value > r.Value ? l : r));
        }

        public IEnumerable<ScoreEntity> GetScores(Guid[]? dancerIds, Guid[]? songIds, bool topScoresOnly)
        {
            var scoresQuery = _context.Scores.AsQueryable();
            if (dancerIds == null || dancerIds.Length == 0) scoresQuery = scoresQuery.Include(s => s.Dancer);
            if (songIds == null || songIds.Length == 0) scoresQuery = scoresQuery.Include(s => s.Song);
            var scores = scoresQuery
                .Where(score => dancerIds == null || dancerIds.Length <= 0 || dancerIds.Contains(score.DancerId))
                .Where(score => songIds == null || songIds.Length <= 0 || songIds.Contains(score.SongId))
                .ToList();
            if (topScoresOnly)
            {
                scores = scores
                    .GroupBy(s => new {s.SongId, s.DancerId})
                    .Select(g => g
                        .OrderByDescending(s => s.Value)
                        .First()
                    ).ToList();
            }

            return scores;
        }

        public IEnumerable<ScoreEntity> GetTopScores(Guid songId)
        {
            return GetScores(null, new Guid[]{songId}, true);
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