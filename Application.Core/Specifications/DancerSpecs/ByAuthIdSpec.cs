using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications.DancerSpecs
{
    public class ByAuthIdSpec : Specification<Dancer>, ISingleResultSpecification
    {
        public ByAuthIdSpec(string id)
        {
            Query
                .Where(e => e.AuthenticationId == id)
                .Take(1);
        }
    }
}