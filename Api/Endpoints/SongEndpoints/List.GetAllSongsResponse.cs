using System.Collections.Generic;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class GetAllSongsResponse
    {
        public IList<SongRecord> songs { get; set; }
    }
}