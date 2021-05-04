using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Types.Summer2021
{
    public class GradedIngredientType : ObjectType<GradedIngredient>
    {
        protected override void Configure(IObjectTypeDescriptor<GradedIngredient> descriptor)
        {
            descriptor
                .Field(t => t.Ingredient)
                .ResolveWith<GradedIngredientResolvers>(t => 
                    t.GetIngredientAsync(default!, default!, default));
            descriptor
                .Field(t => t.IngredientId)
                .ID(nameof(Ingredient));
        }

        private class GradedIngredientResolvers
        {
            public Task<Ingredient> GetIngredientAsync(
                GradedIngredient gradedIngredient,
                IngredientByIdDataLoader ingredientById,
                CancellationToken cancellationToken)
            {
                return ingredientById.LoadAsync(gradedIngredient.IngredientId, cancellationToken);
            }
        }
    }
}