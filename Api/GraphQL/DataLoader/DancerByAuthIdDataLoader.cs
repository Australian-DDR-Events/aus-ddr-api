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

namespace AusDdrApi.GraphQL.DataLoader
{
    public class DancerByAuthIdDataLoader : BatchDataLoader<string, Dancer>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public DancerByAuthIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<DatabaseContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }
        
        protected override async Task<IReadOnlyDictionary<string, Dancer>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            await using DatabaseContext dbContext = 
                _dbContextFactory.CreateDbContext();

            return await dbContext.Dancers
                .AsQueryable()
                .Where(d => keys.Contains(d.AuthenticationId))
                .ToDictionaryAsync(d => d.AuthenticationId, cancellationToken);
        }
    }
}