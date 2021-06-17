using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Specifications.DancerSpecs;
using Xunit;

namespace UnitTests.Core.Specifications.DancerSpecs
{
    public class DancersSpecTests
    {
        [Fact(DisplayName = "If no data is skipped and limit equals number of entries, return full list")]
        public void ReturnFullListIfNoSkipAndEqualLimit()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancersSpec(0, 3);

            var discoveredDancers = spec.Evaluate(items);
            
            Assert.Equal(items.OrderBy(d => d.Id), discoveredDancers);
        }
        
        [Fact(DisplayName = "If no data is skipped and limit is less than number of entries, return shorter list")]
        public void ReturnPartialListIfNoSkipAndShorterLimit()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancersSpec(0, 2);

            var discoveredDancers = spec.Evaluate(items);
            
            Assert.Equal(items.OrderBy(d => d.Id).Take(2), discoveredDancers);
        }
        
        [Fact(DisplayName = "If data is skipped and limit is less than number of entries, return shorter list")]
        public void ReturnPartialListIfSkipAndShorterLimit()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancersSpec(1, 2);

            var discoveredDancers = spec.Evaluate(items);
            
            Assert.Equal(items.OrderBy(d => d.Id).Skip(1).Take(2), discoveredDancers);
        }
        
        [Fact(DisplayName = "If data is skipped and limit is more than than number of entries, return shorter list")]
        public void ReturnPartialListIfSkipAndLongerLimit()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancersSpec(1, 5);

            var discoveredDancers = spec.Evaluate(items);
            
            Assert.Equal(items.OrderBy(d => d.Id).Skip(1).Take(2), discoveredDancers);
        }
        
        [Fact(DisplayName = "If data is skipped passed the list length, return empty list")]
        public void ReturnEmptyListIfSkippedPastEnd()
        {
            var dancer1 = new Dancer { Id = Guid.NewGuid() };
            var dancer2 = new Dancer { Id = Guid.NewGuid() };
            var dancer3 = new Dancer { Id = Guid.NewGuid() };

            var items = new List<Dancer> {dancer1, dancer2, dancer3};

            var spec = new DancersSpec(5, 5);

            var discoveredDancers = spec.Evaluate(items);
            
            Assert.Empty(discoveredDancers);
        }
    }
}