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
                .IdField(song => song.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<SongByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }

        private class SongResolvers
        {
            public async Task<IEnumerable<Course>> GetCoursesAsync(
                Song song,
                [ScopedService] DatabaseContext dbContext,
                CourseByIdDataLoader courseById,
                CancellationToken cancellationToken)
            {
                var courseIds = await dbContext.Songs
                    .Where(s => s.Id == song.Id)
                    .Include(s => s.Courses)
                    .SelectMany(s => s.Courses.Select(c => c.Id))
                    .ToArrayAsync(cancellationToken);
                return await courseById.LoadAsync(courseIds, cancellationToken);
            }
        }
    }
}