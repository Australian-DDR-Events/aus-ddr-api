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
    public class CourseType : ObjectType<Course>
    {
        protected override void Configure(IObjectTypeDescriptor<Course> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(course => course.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<CourseByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }

        private class CourseResolvers
        {
            public async Task<IEnumerable<Song>> GetSongsAsync(
                Course course,
                [ScopedService] DatabaseContext dbContext,
                SongByIdDataLoader songById,
                CancellationToken cancellationToken)
            {
                var songIds = await dbContext.Courses
                    .Where(c => c.Id == course.Id)
                    .Include(c => c.SongDifficulties)
                    .SelectMany(c => c.SongDifficulties.Select(s => s.Id))
                    .ToArrayAsync(cancellationToken);
                return await songById.LoadAsync(songIds, cancellationToken);
            }
        }
    }
}