using System;
using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications
{
    public class DancerByIdSpec : Specification<Dancer>, ISingleResultSpecification
    {
        public DancerByIdSpec(Guid id)
        {
            Query
                .Where(dancer => dancer.Id == id);
        }
    }
}