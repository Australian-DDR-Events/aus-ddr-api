using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradedDancerDishEntity = AusDdrApi.Entities.GradedDancerDish;

namespace AusDdrApi.Services.GradedDancerDish
{
    public interface IGradedDancerDish
    {
        public IEnumerable<GradedDancerDishEntity> GetAllForDancer(Guid dancerId);
        public IEnumerable<GradedDancerDishEntity> GetTopForDancer(Guid dancerId);
        public GradedDancerDishEntity? GetDishForDancer(Guid dishId, Guid dancerId);
        public GradedDancerDishEntity? Get(Guid gradedDancerDishId);
        public Task<GradedDancerDishEntity?> Add(GradedDancerDishEntity gradedDancerDish);
    }
}