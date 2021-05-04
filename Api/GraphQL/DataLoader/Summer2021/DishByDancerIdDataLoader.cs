using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.GraphQL.DataLoader.Summer2021
{
    public class DishByDancerIdDataLoader : BatchDataLoader<Guid, IEnumerable<GradedDancerDish>>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public DishByDancerIdDataLoader(IBatchScheduler batchScheduler,
            IDbContextFactory<DatabaseContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, IEnumerable<GradedDancerDish>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            await using DatabaseContext dbContext = _dbContextFactory.CreateDbContext();

            var gradedDishesForDancer = dbContext.GradedDancerDishes
                .AsQueryable()
                .Where(d => keys.Contains(d.DancerId));
            
            return gradedDishesForDancer
                .AsEnumerable()
                .GroupBy(d => d.DancerId)
                .ToDictionary(d => d.Key, 
                    d => d.AsEnumerable());
        }
    }
}