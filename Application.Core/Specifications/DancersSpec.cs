using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications
{
    public class DancersSpec : Specification<Dancer>
    {
        public DancersSpec(int skip, int limit)
        {
            Query
                .OrderBy(d => d.Id)
                .Skip(skip)
                .Take(limit);
        }
    }
}