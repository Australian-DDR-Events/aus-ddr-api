using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Interface
{
    public interface IAsyncRepository<T> : IRepositoryBase<T> where T : BaseEntity
    {
    }
}