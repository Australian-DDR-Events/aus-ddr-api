using System;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Ardalis.Specification;

namespace Application.Core.Specifications
{
    /// <summary>
    /// ByIdSpec<T> intends to exist as a basic `GetById` spec implementation.
    /// If you require further entity context such as children or filtering,
    /// you can inherit from this spec.
    /// </summary>
    /// <typeparam name="T">A root entity with an ID</typeparam>
    public class ByIdSpec<T> : Specification<T>, ISingleResultSpecification where T : BaseEntity, IAggregateRoot
    {
        public ByIdSpec(Guid id)
        {
            Query
                .Where(e => e.Id == id);
        }
    }
}