using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using DishSongEntity = AusDdrApi.Entities.DishSong;
using DishIngredientEntity = AusDdrApi.Entities.DishIngredient;
using DishEntity = AusDdrApi.Entities.Dish;
using IngredientEntity = AusDdrApi.Entities.Ingredient;

namespace AusDdrApi.Services.Dish
{
    public class DbDish : IDish
    {
        private readonly DatabaseContext _context;

        public DbDish(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<DishEntity> GetAll()
        {
            return _context.Dishes.AsQueryable().ToArray();
        }

        public DishEntity? Get(Guid dishId)
        {
            return _context.Dishes.AsQueryable().SingleOrDefault(d => d.Id == dishId);
        }

        public IEnumerable<IngredientEntity> GetIngredientsForDish(Guid dishId)
        {
            return _context
                .DishIngredients
                .Include(d => d.Ingredient)
                .AsQueryable()
                .Where(d => d.DishId == dishId)
                .Select(d => d.Ingredient!)
                .AsEnumerable();
        }

        public IEnumerable<DishSongEntity> GetSongsForDish(Guid dishId)
        {
            return _context
                .DishSongs
                .AsQueryable()
                .Where(d => d.DishId == dishId)
                .AsEnumerable();
        }

        public async Task<DishEntity> Add(DishEntity dish)
        {
            var dishEntity = await _context.Dishes.AddAsync(dish);
            return dishEntity.Entity;
        }

        public async Task AddDishIngredients(IEnumerable<DishIngredientEntity> dishIngredients)
        {
            await _context
                .DishIngredients
                .AddRangeAsync(dishIngredients);
        }

        public async Task AddDishSongs(IEnumerable<DishSongEntity> dishSongs)
        {
            await _context
                .DishSongs
                .AddRangeAsync(dishSongs);
        }

        public DishEntity Update(DishEntity dish)
        {
            return _context.Dishes.Update(dish).Entity;
        }

        public bool Delete(Guid dishId)
        {
            var dish = Get(dishId);
            if (dish == null) return false;
            _context.Dishes.Remove(dish);
            return true;

        }
    }
}