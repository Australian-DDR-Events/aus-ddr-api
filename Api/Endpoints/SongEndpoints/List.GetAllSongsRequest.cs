using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class GetAllSongsRequest
    {
        [FromQuery]
        public int Page { get; set; } = 0;

        [FromQuery]
        [Range(1, 100, ErrorMessage = "Cannot request more than 100 songs per request")]
        public int Limit { get; set; } = 20;
    }
}