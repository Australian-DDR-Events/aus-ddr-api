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

namespace AusDdrApi.GraphQL.Badges
{
    [ExtendObjectType("Query")]
    public class BadgeQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Badge> GetBadges([ScopedService] DatabaseContext dbContext) => dbContext.Badges;
        
        public Task<Badge?> GetBadgeByIdAsync(
            [ID(nameof(Badge))] Guid id,
            BadgeByIdDataLoader badgeByIdDataLoader,
            CancellationToken cancellationToken) => badgeByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<Badge>> GetBadgesByIdAsync(
            [ID(nameof(Badge))] Guid[] ids,
            BadgeByIdDataLoader badgeByIdDataLoader,
            CancellationToken cancellationToken) => await badgeByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<Badge>();
    }
}