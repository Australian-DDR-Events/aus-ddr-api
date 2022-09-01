using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.SongDifficultyEndpoints
{
    public class CreateSongDifficultyRequest
    {
        [FromRoute]
        public Guid SongId { get; set; }
        [FromBody]
        public string Mode { get; set; }
        [FromBody]
        public string Difficulty { get; set; }
        [FromBody]
        public int Level { get; set; }
        [FromBody]
        public int MaxScore { get; set; }
    }
}