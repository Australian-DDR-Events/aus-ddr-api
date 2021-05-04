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
    public class DishByIdDataLoader : BatchDataLoader<Guid, Dish>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public DishByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<DatabaseContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }
        
        protected override async Task<IReadOnlyDictionary<Guid, Dish>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            await using DatabaseContext dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Dishes
                .AsQueryable()
                .Where(d => keys.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id, cancellationToken);
        }
    }
}