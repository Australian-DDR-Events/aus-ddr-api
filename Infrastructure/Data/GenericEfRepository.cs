using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Api.Core.Entities;
using Api.Core.Interface;

namespace Infrastructure.Data
{
    public class GenericEfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly DatabaseContext _dbContext;

        public GenericEfRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] {id};
            return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public Task<IReadOnlyList<T>> List(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>> List(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Add(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Edit(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}