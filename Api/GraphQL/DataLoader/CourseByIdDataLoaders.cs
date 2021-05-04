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
    public class CourseByIdDataLoader : BatchDataLoader<Guid, Course>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public CourseByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<DatabaseContext> dbContextFactory) : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }
        
        protected override async Task<IReadOnlyDictionary<Guid, Course>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            await using DatabaseContext dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Courses
                .AsQueryable()
                .Where(c => keys.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, cancellationToken);
        }
    }
}