using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.GraphQL.Types.Summer2021
{
    public class GradedDancerDishType : ObjectType<GradedDancerDish>
    {
        protected override void Configure(IObjectTypeDescriptor<GradedDancerDish> descriptor)
        {
            descriptor
                .Field(t => t.Dancer)
                .ResolveWith<GradedDancerDishResolvers>(t => 
                    t.GetDancerAsync(default!, default!, default));
            descriptor
                .Field(t => t.DancerId)
                .ID(nameof(Dancer));
            
            descriptor
                .Field(t => t.GradedDish)
                .ResolveWith<GradedDancerDishResolvers>(t => 
                    t.GetGradedDishAsync(default!, default!, default));
            descriptor
                .Field(t => t.GradedDishId)
                .ID(nameof(GradedDish));
        }

        private class GradedDancerDishResolvers
        {
            public Task<Dancer> GetDancerAsync(
                GradedDancerDish gradedDancerDish,
                DancerByIdDataLoader dancerById,
                CancellationToken cancellationToken)
            {
                return dancerById.LoadAsync(gradedDancerDish.DancerId, cancellationToken);
            }
            
            public Task<GradedDish> GetGradedDishAsync(
                GradedDancerDish gradedDancerDish,
                GradedDishByIdDataLoader gradedDishById,
                CancellationToken cancellationToken)
            {
                return gradedDishById.LoadAsync(gradedDancerDish.GradedDishId, cancellationToken);
            }
            
            public async Task<IEnumerable<Score>> GetScoresAsync(
                GradedDancerDish gradedDancerDish,
                [ScopedService] DatabaseContext dbContext,
                ScoreByIdDataLoader scoreById,
                CancellationToken cancellationToken)
            {
                var scoreIds = await dbContext.GradedDancerDishes
                    .Where(c => c.Id == gradedDancerDish.Id)
                    .Include(c => c.Scores)
                    .SelectMany(c => c.Scores.Select(s => s.Id))
                    .ToArrayAsync(cancellationToken);
                return await scoreById.LoadAsync(scoreIds, cancellationToken);
            }
        }
    }
}