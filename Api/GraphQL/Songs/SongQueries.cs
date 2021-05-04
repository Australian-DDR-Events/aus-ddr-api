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

namespace AusDdrApi.GraphQL.Songs
{
    [ExtendObjectType("Query")]
    public class SongQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Song> GetSongs([ScopedService] DatabaseContext context) => context.Songs;

        public Task<Song?> GetSongByIdAsync(
            [ID(nameof(Song))] Guid id,
            SongByIdDataLoader songByIdDataLoader,
            CancellationToken cancellationToken) => songByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<Song>> GetSongsByIdAsync(
            [ID(nameof(Song))] Guid[] ids,
            SongByIdDataLoader songByIdDataLoader,
            CancellationToken cancellationToken) => await songByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<Song>();
    }
}