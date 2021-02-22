using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DishSongResponse
    {
        public Guid Id { get; set; }
        public int CookingOrder { get; set; }
        public SongResponse? Song { get; set; }
        
        public static DishSongResponse FromEntity(DishSong dishSong) => new DishSongResponse()
        {
            Id = dishSong.Id,
            CookingOrder = dishSong.CookingOrder,
            Song = dishSong.Song != null ? SongResponse.FromEntity(dishSong.Song) : null,
        };
    }
}