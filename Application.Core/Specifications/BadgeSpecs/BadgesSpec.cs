using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications.BadgeSpecs
{
    public class BadgesSpec : Specification<Badge>
    {
        public BadgesSpec(int skip, int limit)
        {
            Query
                .OrderBy(d => d.Id)
                .Skip(skip)
                .Take(limit);
        }
    }
}