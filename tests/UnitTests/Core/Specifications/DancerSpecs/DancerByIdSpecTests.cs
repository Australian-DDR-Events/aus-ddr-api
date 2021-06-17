using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Specifications.DancerSpecs;
using Xunit;

namespace UnitTests.Core.Specifications.DancerSpecs
{
    public class DancerByIdSpecTests
    {
        [Fact(DisplayName = "If data source contains dancer id, then return the dancer")]
        public void ReturnDancerIfFound()
        {
            var searchId = Guid.NewGuid();
            
            var dancer1 = new Dancer { Id = searchId };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancerByIdSpec(searchId);

            var discoveredDancer = items
                .Where(spec.WhereExpressions.First().Compile())
                .FirstOrDefault();
            
            Assert.NotNull(discoveredDancer);
            Assert.Equal(dancer1.Id, discoveredDancer.Id);
        }

        [Fact(DisplayName = "If data source does not contain dancer id, then return null")]
        public void ReturnDancerIfNotFound()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2};

            var spec = new DancerByIdSpec(Guid.NewGuid());

            var discoveredDancer = items
                .Where(spec.WhereExpressions.First().Compile())
                .FirstOrDefault();
            
            Assert.Null(discoveredDancer);
        }
    }
}