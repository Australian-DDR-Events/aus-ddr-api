using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SongEntity = AusDdrApi.Entities.Song;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.Song
{
    public class DbSong : ISong
    {
        private DatabaseContext _context;

        public DbSong(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<SongEntity> GetAll()
        {
            return _context.Songs.AsQueryable().ToList();
        }

        public SongEntity? Get(Guid songId)
        {
            return _context.Songs.AsQueryable().SingleOrDefault(s => s.Id == songId);
        }

        public async Task<SongEntity> Add(SongEntity song)
        {
            var songEntity = await _context.Songs.AddAsync(song);
            return songEntity.Entity;
        }

        public SongEntity Update(SongEntity song)
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