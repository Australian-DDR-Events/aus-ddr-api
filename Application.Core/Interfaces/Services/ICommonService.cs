using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface ICommonService<T> where T : BaseEntity, IAggregateRoot
    {
        public Task<Result<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}