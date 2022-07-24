using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    [ApiController]
    public class List : ControllerBase
    {
        private readonly ISongService _songService;

        public List(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet("/songs")]
        [SwaggerOperation(
            Summary = "Gets a collection of songs",
            Description = "Gets a an amount of songs based on paging, ordered by name",
            OperationId = "Songs.List",
            Tags = new[] { "Songs" })
        ]
        public ActionResult<List<GetAllSongsResponse>> HandleAsync([FromQuery] GetAllSongsRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var songs = _songService.GetSongs(request.Page, request.Limit);
            return Ok(songs.Select(GetAllSongsResponse.Convert));
        }
    }
}