using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DishSongResponse
    {
        public Guid Id { get; set; }
        public int CookingOrder { get; set; }
        public string CookingMethod { get; set; } = string.Empty;
        public SongResponse? Song { get; set; }
        
        public static DishSongResponse FromEntity(DishSong dishSong) => new DishSongResponse
        {
            Id = dishSong.Id,
            CookingOrder = dishSong.CookingOrder,
            CookingMethod = dishSong.CookingMethod,
            Song = dishSong.Song != null ? SongResponse.FromEntity(dishSong.Song) : null
        };
    }
}