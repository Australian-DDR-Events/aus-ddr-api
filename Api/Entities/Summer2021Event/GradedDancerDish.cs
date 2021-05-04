using System;
using System.Collections.Generic;
using AusDdrApi.GraphQL.Types;
using AusDdrApi.GraphQL.Types.Summer2021;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class GradedDancerDish
    {
        public Guid Id { get; set; }
        
        public Guid GradedDishId { get; set; }
        
        [GraphQLType(typeof(NonNullType<GradedDishType>))]
        public GradedDish? GradedDish { get; set; }
        
        public Guid DancerId { get; set; }
        [GraphQLType(typeof(NonNullType<DancerType>))]
        public Dancer? Dancer { get; set; }

        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ScoreType>>>))]
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}