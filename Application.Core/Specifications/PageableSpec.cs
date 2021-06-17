using Application.Core.Entities;
using Application.Core.Interfaces;
using Ardalis.Specification;

namespace Application.Core.Specifications
{
    /// <summary>
    /// PageableSpec<T> intends to exist as a basic `List` spec implementation
    /// with simple paging capabilities. If you require further entity context
    /// such as children or filtering, or more complex paging capabilities, you
    /// can inherit from this spec.
    /// </summary>
    /// <typeparam name="T">A root entity with an ID</typeparam>
    public class PageableSpec<T> : Specification<T> where T : BaseEntity, IAggregateRoot
    {
        public PageableSpec(int skip, int limit)
        {
            Query
                .OrderBy(d => d.Id)
                .Skip(skip)
                .Take(limit);
        }
    }
}