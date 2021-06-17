using Application.Core.Entities;
using Application.Core.Interfaces;
using Ardalis.Specification;

namespace Application.Core.Specifications
{
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