using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreEntity = AusDdrApi.Entities.Score;

namespace AusDdrApi.Services.Score
{
    public interface IScore
    {
        public ScoreEntity? Get(Guid scoreId);
        public IEnumerable<ScoreEntity> GetScores(Guid? dancerId, Guid? songId);

        public Task<ScoreEntity> Add(ScoreEntity score);
        public bool Delete(Guid scoreId);

    }
}