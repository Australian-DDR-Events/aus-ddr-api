using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Entities;

namespace AusDdrApi.Services.Entities.ScoreService
{
    public interface IScoreService
    {
        public Score? GetScore(Guid scoreId);
        public IEnumerable<Score> GetScores(Guid? dancerId, Guid? songId);

        public Task<Score> Add(Score score);
    }
}
