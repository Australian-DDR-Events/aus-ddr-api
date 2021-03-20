using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreEntity = AusDdrApi.Entities.Score;

namespace AusDdrApi.Services.Score
{
    public interface IScore
    {
        public ScoreEntity? Get(Guid scoreId);
        public IEnumerable<ScoreEntity> GetTopScores(Guid[] songIds);

        public IEnumerable<ScoreEntity> GetScores(Guid[]? dancerIds, Guid[]? songIds, bool topScoresOnly);
        public IEnumerable<ScoreEntity> GetTopScores(Guid songId);


        public Task<ScoreEntity> Add(ScoreEntity score);
        public bool Delete(Guid scoreId);

    }
}
