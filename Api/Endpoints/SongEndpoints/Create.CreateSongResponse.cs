using System;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class CreateSongResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
    }
}