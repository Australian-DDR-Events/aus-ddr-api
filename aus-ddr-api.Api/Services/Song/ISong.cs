using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SongEntity = AusDdrApi.Entities.Song;

namespace AusDdrApi.Services.Song
{
    public interface ISong
    {
        public IEnumerable<SongEntity> GetAll();
        public SongEntity? Get(Guid songId);

        public Task<SongEntity> Add(SongEntity song);
        public SongEntity? Update(SongEntity song);
        public bool Delete(Guid songId);
    }
}