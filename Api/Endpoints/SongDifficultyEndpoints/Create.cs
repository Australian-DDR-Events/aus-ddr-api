using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Application.Core.Models.SongDifficulties;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongDifficultyEndpoints
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly ISongDifficultyService _songDifficultyService;

        public Create(ISongDifficultyService songDifficultyService)
        {
            _songDifficultyService = songDifficultyService;
        }

        [HttpPost("/song/{songId}/difficulty")]
        [SwaggerOperation(
            Summary = "Add a new song difficulty",
            Description = "",
            OperationId = "",
            Tags = new[] { "Song" })
        ]
        [Authorize]
        [Admin]
        public async Task<ActionResult> HandleAsync([FromBody] [FromRoute] CreateSongDifficultyRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var parseDifficultyResult = Enum.TryParse(request.Difficulty, out Difficulty difficulty);
            var parseModeResult = Enum.TryParse(request.Mode, out PlayMode mode);
            if (!parseDifficultyResult || !parseModeResult) return BadRequest();

            var requestModel = new CreateSongDifficultyRequestModel
            {
                SongId = request.SongId,
                Difficulty = difficulty,
                Mode = mode,
                MaxScore = request.MaxScore,
                Level = request.Level
            };

            var result = await _songDifficultyService.CreateSongDifficulty(requestModel, cancellationToken);
            return result ? Accepted() : BadRequest();
        }
    }
}