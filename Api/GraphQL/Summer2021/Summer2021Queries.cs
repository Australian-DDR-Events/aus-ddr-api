using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

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
        
    }
}