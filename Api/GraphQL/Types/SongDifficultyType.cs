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
    public class SongDifficultyType : ObjectType<SongDifficulty>
    {
        protected override void Configure(IObjectTypeDescriptor<SongDifficulty> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(SongDifficulty => SongDifficulty.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<SongDifficultyByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(s => s.Song)
                .ResolveWith<SongDifficultyResolvers>(t => t.GetSongAsync(default!, default!, default!))
                .UseDbContext<DatabaseContext>();

            descriptor
                .Field(s => s.Scores)
                .ResolveWith<SongDifficultyResolvers>(t => t.GetScoresAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();

            descriptor
                .Field(s => s.TopScore)
                .ResolveWith<SongDifficultyResolvers>(t => t.GetTopScoreAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();


            descriptor
                .Field(s => s.DancerTopScores)
                .ResolveWith<SongDifficultyResolvers>(t => t.GetDancerTopScoresAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();

            descriptor
                .Field(s => s.Courses)
                .ResolveWith<SongDifficultyResolvers>(t => t.GetCoursesAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
        }

        private class SongDifficultyResolvers
        {
            public Task<Song> GetSongAsync(
                SongDifficulty songDifficulty,
                SongByIdDataLoader songById,
                CancellationToken cancellationToken)
            {
                return songById.LoadAsync(songDifficulty.SongId, cancellationToken);
            }
            
            public async Task<IEnumerable<Course>> GetCoursesAsync(
                SongDifficulty songDifficulty,
                [ScopedService] DatabaseContext dbContext,
                CourseByIdDataLoader courseById,
                CancellationToken cancellationToken)
            {
                var courseIds = await dbContext.SongDifficulties
                    .Where(s => s.Id == songDifficulty.Id)
                    .Include(s => s.Courses)
                    .SelectMany(s => s.Courses.Select(c => c.Id))
                    .ToArrayAsync(cancellationToken);
                return await courseById.LoadAsync(courseIds, cancellationToken);
            }
            
            public async Task<IEnumerable<Score>> GetScoresAsync(
                SongDifficulty songDifficulty,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = await dbContext.SongDifficulties
                    .Where(s => s.Id == songDifficulty.Id)
                    .Include(s => s.Scores)
                    .SelectMany(s => s.Scores.Select(c => c.Id))
                    .ToArrayAsync(cancellationToken);
                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
            
            public async Task<Score?> GetTopScoreAsync(
                SongDifficulty songDifficulty,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var score = dbContext.Scores
                    .Where(s => s.SongDifficultyId == songDifficulty.Id)
                    .OrderByDescending(s => s.Value)
                    .ThenByDescending(s => s.SubmissionTime)
                    .FirstOrDefault();
                return score == null ? null : await scoreById.LoadAsync(score.Id, cancellationToken);
            }
            
            public async Task<IEnumerable<Score>> GetDancerTopScoresAsync(
                SongDifficulty songDifficulty,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = dbContext.Scores
                    .Where(s => s.SongDifficultyId == songDifficulty.Id)
                    .ToList()
                    .GroupBy(s => new {s.DancerId})
                    .Select(g => g
                        .OrderByDescending(s => s.Value)
                        .ThenByDescending(s => s.SubmissionTime)
                        .First()
                    )
                    .Select(s => s.Id)
                    .ToArray();
                
                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
        }
    }
}