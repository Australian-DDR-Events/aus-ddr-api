using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Scores
{
    [ExtendObjectType("Query")]
    public class ScoreQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Score> GetScores([ScopedService] DatabaseContext context) => context.Scores;

        public Task<Score?> GetScoreByIdAsync(
            [ID(nameof(Score))] Guid id,
            ScoreByIdDataLoader scoreByIdDataLoader,
            CancellationToken cancellationToken) => scoreByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<Score>> GetScoresByIdAsync(
            [ID(nameof(Dancer))] Guid[] ids,
            ScoreByIdDataLoader scoreByIdDataLoader,
            CancellationToken cancellationToken) => await scoreByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<Score>();
    }
}