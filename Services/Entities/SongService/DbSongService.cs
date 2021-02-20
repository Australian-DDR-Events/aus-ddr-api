using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.Entities.SongService
{
    public class DbSongService : ISongService
    {
        private DatabaseContext _context;

        public DbSongService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Song> GetAll()
        {
            return _context.Songs.AsQueryable().ToArray();
        }

        public Song? Get(Guid songId)
        {
            return _context.Songs.AsQueryable().SingleOrDefault(s => s.Id == songId);
        }

        public async Task<Song> Add(Song song)
        {
            var songEntity = await _context.Songs.AddAsync(song);
            return songEntity.Entity;
        }

        public async Task<Song> Update(Song song)
        {
            return _context.Songs.Update(song).Entity;
        }

        public bool Delete(Guid songId)
        {
            var song = Get(songId);
            if (song != null)
            {
                _context.Songs.Remove(song);
                return true;
            }

            return false;
        }
    }
}