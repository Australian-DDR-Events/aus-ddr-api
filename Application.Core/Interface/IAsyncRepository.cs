using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interface
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> List(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> List(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task Add(T entity, CancellationToken cancellationToken = default);
        Task Delete(T entity, CancellationToken cancellationToken = default);
        Task Edit(T entity, CancellationToken cancellationToken = default);
    }
}