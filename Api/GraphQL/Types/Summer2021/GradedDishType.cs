using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Types.Summer2021
{
    public class GradedDishType : ObjectType<GradedDish>
    {
        protected override void Configure(IObjectTypeDescriptor<GradedDish> descriptor)
        {
            descriptor
                .Field(t => t.Dish)
                .ResolveWith<GradedDishResolvers>(t => 
                    t.GetDishAsync(default!, default!, default));
            descriptor
                .Field(t => t.DishId)
                .ID(nameof(Dish));
        }

        private class GradedDishResolvers
        {
            public Task<Dish> GetDishAsync(
                GradedDish gradedDish,
                DishByIdDataLoader dishById,
                CancellationToken cancellationToken)
            {
                return dishById.LoadAsync(gradedDish.DishId, cancellationToken);
            }
        }
    }
}