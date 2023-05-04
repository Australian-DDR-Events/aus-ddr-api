using System.Threading;
using Application.Core.Interfaces.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.SongEndpoints;

[ApiController]
public class GetById : ControllerBase
{
    private readonly ISongService _songService;

    public GetById(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet("/songs/{Id:guid}")]
    [SwaggerOperation(
        Summary = "Gets a Song",
        Description = "Gets a single Song with all associated charts",
        OperationId = "Songs.GetById",
        Tags = new[] { "Songs" })
    ]
    public ActionResult<GetSongByIdResponse> HandleAsync([FromRoute] GetSongByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = _songService.GetSong(request.Id);
        if (result.Status == ResultStatus.NotFound) return NotFound();
        return Ok(GetSongByIdResponse.Convert(result.Value));
    }
}
