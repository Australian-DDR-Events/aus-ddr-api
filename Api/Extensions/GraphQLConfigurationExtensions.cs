using AusDdrApi.GraphQL.Badges;
using AusDdrApi.GraphQL.Courses;
using AusDdrApi.GraphQL.Dancers;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using AusDdrApi.GraphQL.Scores;
using AusDdrApi.GraphQL.SongDifficulties;
using AusDdrApi.GraphQL.Songs;
using AusDdrApi.GraphQL.Summer2021;
using AusDdrApi.GraphQL.Types;
using AusDdrApi.GraphQL.Types.Summer2021;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable InconsistentNaming

namespace AusDdrApi.Extensions
{
    public static class GraphQLConfigurationExtensions
    {
        public static IServiceCollection AddGraphQLConfiguration(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()

                // Queries
                .AddQueryType(x => x.Name("Query"))
                .AddTypeExtension<DancerQueries>()
                .AddTypeExtension<SongQueries>()
                .AddTypeExtension<SongDifficultyQueries>()
                .AddTypeExtension<CourseQueries>()
                .AddTypeExtension<ScoreQueries>()
                .AddTypeExtension<Summer2021Queries>()
                .AddTypeExtension<BadgeQueries>()
                
                // Mutations
                .AddMutationType(x => x.Name("Mutation"))
                .AddTypeExtension<SongMutations>()
                .AddTypeExtension<BadgeMutations>()
                .AddTypeExtension<CourseMutations>()
                .AddTypeExtension<DancerMutations>()

                // Types
                .AddType(new UuidType('D'))
                .AddType<UploadType>()
                .AddType<DancerType>()
                .AddType<SongDifficultyType>()
                .AddType<CourseType>()
                .AddType<ScoreType>()
                .AddType<BadgeType>()
                .AddType<GradedIngredientType>()
                .AddType<GradedDancerIngredientType>()
                .AddType<GradedDishType>()
                .AddType<GradedDancerDishType>()

                // Extensions
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .EnableRelaySupport()
                .AddAuthorization()

                // Data loaders
                .AddDataLoader<DancerByIdDataLoader>()
                .AddDataLoader<DancerByAuthIdDataLoader>()
                .AddDataLoader<BadgeByIdDataLoader>()
                .AddDataLoader<SongByIdDataLoader>()
                .AddDataLoader<SongDifficultyByIdDataLoader>()
                .AddDataLoader<CourseByIdDataLoader>()
                .AddDataLoader<ScoreByIdDataLoader>()
                .AddDataLoader<IngredientByIdDataLoader>()
                .AddDataLoader<GradedIngredientByIdDataLoader>()
                .AddDataLoader<IngredientByDancerIdDataLoader>()
                .AddDataLoader<GradedDishByIdDataLoader>()
                .AddDataLoader<DishByDancerIdDataLoader>();
            
            return services;
        }
    }
}