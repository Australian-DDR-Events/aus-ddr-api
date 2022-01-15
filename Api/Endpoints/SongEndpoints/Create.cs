using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    public class Create : EndpointWithResponse<CreateSongRequest, CreateSongResponse, Song>
    {
        private const string Route = "/songs";
        private readonly ISongService _songService;

        public Create(ISongService songService)
        {
            _songService = songService;
        }

        [HttpPost(Route)]
        [SwaggerOperation(
            Summary = "Add a new song",
            Description = "",
            OperationId = "",
            Tags = new[] { "Song" })
        ]
        [Authorize]
        public override async Task<ActionResult<CreateSongResponse>> HandleAsync(CreateSongRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var created = await _songService.CreateSongAsync(new Song
            {
                Artist = request.Artist,
                Name = request.Name
            }, cancellationToken);
            return Created(new Uri("/"), Convert(created.Value));
        }

        public override CreateSongResponse Convert(Song u)
        {
            return new CreateSongResponse
            {
                Name = u.Name,
                Artist = u.Artist,
                Id = u.Id
            };
        }
    }
}