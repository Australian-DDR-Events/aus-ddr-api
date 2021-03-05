using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DishSongEntity = AusDdrApi.Entities.DishSong;
using DishEntity = AusDdrApi.Entities.Dish;
using IngredientEntity = AusDdrApi.Entities.Ingredient;

namespace AusDdrApi.Services.Dish
{
    public interface IDish
    {
        public IEnumerable<DishEntity> GetAll();
        public DishEntity? Get(Guid dishId);
        public IEnumerable<DishSongEntity> GetSongsForDish(Guid dishId);

        public Task<DishEntity> Add(DishEntity dish);
        public void AddDishIngredients(Guid dishId, IEnumerable<IngredientEntity> ingredients);
        public Task AddDishSongs(IEnumerable<DishSongEntity> dishSongs);
        public DishEntity Update(DishEntity dish);
        public bool Delete(Guid dishId);
    }
}