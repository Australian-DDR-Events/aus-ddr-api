namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class CreateSongRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
    }
}