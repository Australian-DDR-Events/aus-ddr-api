using System;
using System.Collections.Generic;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class GetAllSongsResponse
    {
        private GetAllSongsResponse(Guid id, string name, string artist)
        {
            Id = id;
            Name = name;
            Artist = artist;
        }
        
        public Guid Id { get; }
        public string Name { get; }
        public string Artist { get; }

        public static GetAllSongsResponse Convert(Song song) =>
            new GetAllSongsResponse(song.Id, song.Name, song.Artist);
    }
}