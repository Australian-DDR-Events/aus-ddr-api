using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.GraphQL.Types
{
    public class SongType : ObjectType<Song>
    {
        protected override void Configure(IObjectTypeDescriptor<Song> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(score => score.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<SongByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(s => s.SongDifficulties)
                .ResolveWith<SongResolvers>(t => t.GetSongDifficultiesAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
        }

        private class SongResolvers
        {
            public async Task<IEnumerable<SongDifficulty>> GetSongDifficultiesAsync(
                Song song,
                [ScopedService] DatabaseContext dbContext,
                SongDifficultyByIdDataLoader songDifficultyById,
                CancellationToken cancellationToken)
            {
                var songDifficultyIds = await dbContext.Songs
                    .Where(s => s.Id == song.Id)
                    .Include(s => s.SongDifficulties)
                    .SelectMany(s => s.SongDifficulties.Select(c => c.Id))
                    .ToArrayAsync(cancellationToken);
                return await songDifficultyById.LoadAsync(songDifficultyIds, cancellationToken);
            }
        }
    }
}