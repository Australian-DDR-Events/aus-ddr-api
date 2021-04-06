using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Types.Summer2021
{
    public class GradedDancerIngredientType : ObjectType<GradedDancerIngredient>
    {
        protected override void Configure(IObjectTypeDescriptor<GradedDancerIngredient> descriptor)
        {
            descriptor
                .Field(t => t.Score)
                .ResolveWith<GradedDancerIngredientResolvers>(t => 
                    t.GetScoreAsync(default!, default!, default));
            descriptor
                .Field(t => t.ScoreId)
                .ID(nameof(Score));
            
            descriptor
                .Field(t => t.GradedIngredient)
                .ResolveWith<GradedDancerIngredientResolvers>(t => 
                    t.GetGradedIngredientAsync(default!, default!, default));
            descriptor
                .Field(t => t.GradedIngredientId)
                .ID(nameof(GradedIngredient));
        }

        private class GradedDancerIngredientResolvers
        {
            public Task<Score> GetScoreAsync(
                GradedDancerIngredient gradedDancerIngredient,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                return scoreById.LoadAsync(gradedDancerIngredient.ScoreId, cancellationToken);
            }
            
            public Task<GradedIngredient> GetGradedIngredientAsync(
                GradedDancerIngredient gradedDancerIngredient,
                GradedIngredientByIdDataLoader gradedIngredientById,
                CancellationToken cancellationToken)
            {
                return gradedIngredientById.LoadAsync(gradedDancerIngredient.GradedIngredientId, cancellationToken);
            }
        }
    }
}