using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints;

public class GetById : EndpointWithResponse<GetSongWithTopScoresRequest, GetSongWithTopScoresResponse, Song>
{
    private readonly ISongService _songService;

    public GetById(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet(GetSongWithTopScoresRequest.Route)]
    [SwaggerOperation(
        Summary = "Gets a Song with top 3 scores",
        Description = "Gets a single Song with all associated difficulties and up to 3 scores",
        OperationId = "Songs.GetById",
        Tags = new[] { "Songs" })
    ]
    public override async Task<ActionResult<GetSongWithTopScoresResponse>> HandleAsync([FromRoute] [FromQuery] GetSongWithTopScoresRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _songService.GetSong(request.Id, request.WithTopScores, cancellationToken);

        return this.ConvertToActionResult(result);
    }

    public override GetSongWithTopScoresResponse Convert(Song u)
    {
        return new GetSongWithTopScoresResponse(u.Id, u.Name, u.Artist, GetSongDifficulties(u.SongDifficulties));
    }

    private IEnumerable<SongDifficultyApiResponse> GetSongDifficulties(IEnumerable<SongDifficulty>? difficulties)
    {
        if (difficulties == null) return new List<SongDifficultyApiResponse>();

        return difficulties.Select(d =>
        {
            return new SongDifficultyApiResponse(d.Id, d.PlayMode, d.Difficulty, d.Level,
                d.Scores.Select(s => new ScoreApiResponse(s.Value, s.DancerId)));
        });
    }
}