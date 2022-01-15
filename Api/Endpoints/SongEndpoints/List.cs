using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class List : EndpointWithResponse<GetAllSongsRequest, GetAllSongsResponse, IEnumerable<Song>>
    {
        private const string Route = "/songs";
        private readonly ISongService _songService;

        public List(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet(Route)]
        [SwaggerOperation()]
        public override async Task<ActionResult<GetAllSongsResponse>> HandleAsync([FromQuery] GetAllSongsRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var songs = await _songService.GetSongsAsync(request.Page, request.Limit, cancellationToken);
            return Ok(songs);
        }

        public override GetAllSongsResponse Convert(IEnumerable<Song> u)
        {
            return new GetAllSongsResponse { songs = u.Select(x => new SongRecord(x.Name, x.Artist)).ToList() };
        }
    }
}