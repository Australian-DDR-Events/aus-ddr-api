using System;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Ardalis.Specification;

namespace Application.Core.Specifications.DancerSpecs
{
    public class ByAuthIdSpec<T> : Specification<T>, ISingleResultSpecification where T : Dancer, IAggregateRoot
    {
        public ByAuthIdSpec(string authId)
        {
            Query
                .Where(e => e.AuthenticationId == authId);
        }
    }
}