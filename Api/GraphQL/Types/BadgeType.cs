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
    public class BadgeType : ObjectType<Badge>
    {
        protected override void Configure(IObjectTypeDescriptor<Badge> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(badge => badge.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<BadgeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(b => b.Dancers)
                .ResolveWith<BadgeResolvers>(t => t.GetDancersAsync(default!, default!, default!, default!))
                .UseDbContext<DatabaseContext>();
        }
    
        private class BadgeResolvers
        {
            public async Task<IEnumerable<Dancer>> GetDancersAsync(
                Badge badge,
                [ScopedService] DatabaseContext dbContext,
                DancerByIdDataLoader dancerById,
                CancellationToken cancellationToken)
            {
                var dancerIds = await dbContext.Badges
                    .Where(b => b.Id == badge.Id)
                    .Include(b => b.Dancers)
                    .SelectMany(b => b.Dancers.Select(d => d.Id))
                    .ToArrayAsync(cancellationToken);

                return await dancerById.LoadAsync(dancerIds, cancellationToken);
            }
        }
    }
}