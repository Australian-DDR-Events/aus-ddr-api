using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Song;
using AusDdrApi.Attributes;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly ISongService _songService;

        public Create(ISongService songService)
        {
            _songService = songService;
        }

        [HttpPost("/songs")]
        [SwaggerOperation(
            Summary = "Add a new song",
            Description = "",
            OperationId = "",
            Tags = new[] { "Song" })
        ]
        [Authorize]
        [Admin]
        public async Task<ActionResult> HandleAsync(CreateSongRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var requestModel = new CreateSongRequestModel
            {
                Name = request.Name,
                Artist = request.Artist,
                KonamiId = request.KonamiId
            };
            await _songService.CreateSongAsync(requestModel, cancellationToken);
            return Accepted();
        }
    }
}