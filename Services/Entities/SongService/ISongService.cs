using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Entities;

namespace AusDdrApi.Services.Entities.SongService
{
    public interface ISongService
    {
        public IEnumerable<Song> GetAll();
        public Song? Get(Guid songId);

        public Task<Song> Add(Song song);
        public Task<Song?> Update(Song song);
        public bool Delete(Guid songId);
    }
}