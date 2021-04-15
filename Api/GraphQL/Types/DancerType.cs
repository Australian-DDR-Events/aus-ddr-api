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
    public class DancerType : ObjectType<Dancer>
    {
        protected override void Configure(IObjectTypeDescriptor<Dancer> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(dancer => dancer.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<DancerByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(d => d.Badges)
                .ResolveWith<DancerResolvers>(t => t.GetBadgesAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
            
            descriptor
                .Field(d => d.Scores)
                .ResolveWith<DancerResolvers>(t => t.GetScoresAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
        }

        private class DancerResolvers
        {
            public async Task<IEnumerable<Badge>> GetBadgesAsync(
                Dancer dancer,
                [ScopedService] DatabaseContext dbContext,
                BadgeByIdDataLoader badgeById,
                CancellationToken cancellationToken)
            {
                var badgeIds = await dbContext.Dancers
                    .Where(d => d.Id == dancer.Id)
                    .Include(d => d.Badges)
                    .SelectMany(d => d.Badges.Select(t => t.Id))
                    .ToArrayAsync(cancellationToken);

                return await badgeById.LoadAsync(badgeIds, cancellationToken);
            }
            public async Task<IEnumerable<Score>> GetScoresAsync(
                Dancer dancer,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = await dbContext.Dancers
                    .Where(d => d.Id == dancer.Id)
                    .Include(d => d.Scores)
                    .SelectMany(d => d.Scores.Select(t => t.Id))
                    .ToArrayAsync(cancellationToken);

                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
        }
    }
}