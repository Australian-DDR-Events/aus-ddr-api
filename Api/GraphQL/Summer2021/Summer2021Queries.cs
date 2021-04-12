using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Summer2021
{
    [ExtendObjectType("Query")]
    public class Summer2021Queries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Ingredient> GetIngredients([ScopedService] DatabaseContext context) => context.Ingredients;
        
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Dish> GetDishes([ScopedService] DatabaseContext context) => context.Dishes;

        public Task<IEnumerable<GradedDancerIngredient>> GetIngredientsByDancerId(
            [ID(nameof(Dancer))] Guid id,
            IngredientByDancerIdDataLoader ingredientByDancerIdDataLoader,
            CancellationToken cancellationToken) => ingredientByDancerIdDataLoader.LoadAsync(id, cancellationToken);

    }
}