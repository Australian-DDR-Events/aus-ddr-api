using System.Threading;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Microsoft.AspNetCore.Http;
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
        return result.ResultCode switch
        {
            ResultCode.Ok => Ok(GetSongByIdResponse.Convert(result.Value.Value)),
            ResultCode.NotFound => NotFound(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
