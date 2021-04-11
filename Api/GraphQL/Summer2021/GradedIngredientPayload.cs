using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Summer2021
{
    public class GradedIngredientPayload : Payload
    {
        public GradedIngredientPayload(GradedDancerIngredient gradedDancerIngredient)
        {
            GradedDancerIngredient = gradedDancerIngredient;
        }
        
        public GradedIngredientPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
        
        public GradedDancerIngredient? GradedDancerIngredient { get; }
    }
}