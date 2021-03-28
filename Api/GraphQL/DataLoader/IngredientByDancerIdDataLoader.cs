using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.GraphQL.DataLoader
{
    public class IngredientByDancerIdDataLoader : BatchDataLoader<Guid, IEnumerable<GradedDancerIngredient>>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public IngredientByDancerIdDataLoader(IBatchScheduler batchScheduler,
            IDbContextFactory<DatabaseContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, IEnumerable<GradedDancerIngredient>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            await using DatabaseContext dbContext =
                _dbContextFactory.CreateDbContext();

            return await dbContext.GradedDancerIngredients
                .AsQueryable()
                .Where(d => keys.Contains(d.DancerId))
                .GroupBy(d => d.DancerId)
                .ToDictionaryAsync(d => d.Key, 
                    d => d.AsEnumerable(), 
                    cancellationToken);
        }
    }
}