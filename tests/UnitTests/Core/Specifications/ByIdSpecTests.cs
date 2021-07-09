using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Specifications;
using Xunit;

namespace UnitTests.Core.Specifications
{
    public class ByIdSpecTests
    {
        [Fact(DisplayName = "If data source contains id, then return the entity")]
        public void ReturnEntityIfFound()
        {
            var searchId = Guid.NewGuid();
            
            var dancer1 = new Dancer { Id = searchId };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new ByIdSpec<Dancer>(searchId);

            var discoveredDancer = items
                .Where(spec.WhereExpressions.First().Compile())
                .FirstOrDefault();
            
            Assert.NotNull(discoveredDancer);
            Assert.Equal(dancer1.Id, discoveredDancer.Id);
        }

        [Fact(DisplayName = "If data source does not contain id, then return null")]
        public void ReturnDancerIfNotFound()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2};

            var spec = new ByIdSpec<Dancer>(Guid.NewGuid());

            var discoveredDancer = items
                .Where(spec.WhereExpressions.First().Compile())
                .FirstOrDefault();
            
            Assert.Null(discoveredDancer);
        }
    }
}