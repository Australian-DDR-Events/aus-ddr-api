using System;
using AusDdrApi.GraphQL.Types;
using AusDdrApi.GraphQL.Types.Summer2021;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class GradedDancerIngredient
    {
        public Guid Id { get; set; }
        
        public Guid GradedIngredientId { get; set; }
        [GraphQLType(typeof(NonNullType<GradedIngredientType>))]
        public GradedIngredient GradedIngredient { get; set; }
        
        public Guid DancerId { get; set; }
        [GraphQLType(typeof(NonNullType<DancerType>))]
        public Dancer Dancer { get; set; }
        
        public Guid ScoreId { get; set; }
        [GraphQLType(typeof(NonNullType<ScoreType>))]
        public Score Score { get; set; }
    }
}