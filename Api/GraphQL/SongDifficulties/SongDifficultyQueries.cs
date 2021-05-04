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

namespace AusDdrApi.GraphQL.SongDifficulties
{
    [ExtendObjectType("Query")]
    public class SongDifficultyQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<SongDifficulty> GetSongDifficulties([ScopedService] DatabaseContext context) => context.SongDifficulties;

        public Task<SongDifficulty?> GetSongDifficultyByIdAsync(
            [ID(nameof(SongDifficulty))] Guid id,
            SongDifficultyByIdDataLoader songDifficultyByIdDataLoader,
            CancellationToken cancellationToken) => songDifficultyByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<SongDifficulty>> GetSongDifficultiesByIdAsync(
            [ID(nameof(SongDifficulty))] Guid[] ids,
            SongDifficultyByIdDataLoader songDifficultyByIdDataLoader,
            CancellationToken cancellationToken) => await songDifficultyByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<SongDifficulty>();
    }
}