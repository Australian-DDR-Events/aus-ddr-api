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

            descriptor
                .Field(s => s.Scores)
                .ResolveWith<SongResolvers>(t => t.GetScoresAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();

            descriptor
                .Field(s => s.DancerTopScores)
                .ResolveWith<SongResolvers>(t => t.GetDancerTopScoresAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
            
            descriptor
                .Field(s => s.Courses)
                .ResolveWith<SongResolvers>(t => t.GetCoursesAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
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
            
            public async Task<IEnumerable<Score>> GetScoresAsync(
                Song song,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = await dbContext.Songs
                    .Where(s => s.Id == song.Id)
                    .Include(s => s.Scores)
                    .SelectMany(s => s.Scores.Select(c => c.Id))
                    .ToArrayAsync(cancellationToken);
                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
            
            public async Task<IEnumerable<Score>> GetDancerTopScoresAsync(
                Song song,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = dbContext.Scores
                    .Where(s => s.SongId == song.Id)
                    .ToList()
                    .GroupBy(s => new {s.DancerId})
                    .Select(g => g
                        .OrderByDescending(s => s.Value)
                        .ThenByDescending(s => s.SubmissionTime)
                        .First()
                    ).OrderByDescending(s => s.Value)
                    .ThenByDescending(s => s.SubmissionTime)
                    .Select(s => s.Id)
                    .ToArray();
                
                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
        }
    }
}