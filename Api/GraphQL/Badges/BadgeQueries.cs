using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

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
    }
}