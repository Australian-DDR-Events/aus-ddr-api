using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using DishSongEntity = AusDdrApi.Entities.DishSong;
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
            return _context
                .Dishes
                .Include(d => d.DishSongs)
                .ThenInclude(s => s.Song)
                .Include(d => d.Ingredients)
                .AsSplitQuery()
                .ToList();
        }

        public DishEntity? Get(Guid dishId)
        {
            return _context
                .Dishes
                .Include(d => d.DishSongs)
                .ThenInclude(s => s.Song)
                .Include(d => d.Ingredients)
                .AsSplitQuery()
                .AsQueryable().SingleOrDefault(d => d.Id == dishId);
        }

        public IEnumerable<DishSongEntity> GetSongsForDish(Guid dishId)
        {
            return _context
                .DishSongs
                .Include(d => d.Song)
                .AsQueryable()
                .Where(d => d.DishId == dishId)
                .ToList();
        }

        public async Task<DishEntity> Add(DishEntity dish)
        {
            var dishEntity = await _context.Dishes.AddAsync(dish);
            return dishEntity.Entity;
        }

        public void AddDishIngredients(Guid dishId, IEnumerable<IngredientEntity> ingredients)
        {
            var dish = _context
                .Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefault(d => d.Id == dishId);
            if (dish != null)
            {
                foreach (var ingredient in ingredients)
                {
                    dish.Ingredients.Add(ingredient);
                }
            }
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