using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Interfaces
{
    public interface IAsyncRepository<T> : IRepositoryBase<T> where T : BaseEntity, IAggregateRoot
    {
    }
}