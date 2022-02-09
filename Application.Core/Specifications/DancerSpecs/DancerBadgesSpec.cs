using System;
using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications.DancerSpecs;

public sealed class DancerBadgesSpec : Specification<Dancer>, ISingleResultSpecification
{
    public DancerBadgesSpec(Guid id)
    {
        Query
            .Where(d => d.Id.Equals(id))
            .Include(d => d.Badges)
            .ThenInclude(b => b.Event)
            .Take(1);
    }
}