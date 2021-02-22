using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradedDishEntity = AusDdrApi.Entities.GradedDish;

namespace AusDdrApi.Services.GradedDish
{
    public interface IGradedDish
    {
        public IEnumerable<GradedDishEntity> GetAllForDish(Guid dishId);
        public GradedDishEntity? Get(Guid gradedDishId);

        public Task<GradedDishEntity> Add(GradedDishEntity gradedDish);
        public GradedDishEntity? Update(GradedDishEntity gradedDish);
        public bool Delete(Guid gradedDishId);
    }
}