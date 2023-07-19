using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Charts;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.ChartEndpoints;

[ApiController]
public class Create : ControllerBase
{
    private readonly IChartService _chartService;

    public Create(IChartService chartService)
    {
        _chartService = chartService;
    }

    [HttpPost("/song/{songId}/chart")]
    [SwaggerOperation(
        Summary = "Add a new song chart",
        Description = "",
        OperationId = "",
        Tags = new[] { "Song" })
    ]
    [Authorize]
    [Admin]
    public async Task<ActionResult> HandleAsync([FromBody] [FromRoute] CreateChartRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var parseChartResult = Enum.TryParse(request.Difficulty, out Difficulty difficulty);
        var parseModeResult = Enum.TryParse(request.Mode, out PlayMode mode);
        if (!parseChartResult || !parseModeResult) return BadRequest();

        var requestModel = new CreateChartRequestModel
        {
            SongId = request.SongId,
            Difficulty = difficulty,
            Mode = mode,
            MaxScore = request.MaxScore,
            Level = request.Level
        };

        var result = await _chartService.CreateChart(requestModel, cancellationToken);
        return result ? Accepted() : BadRequest();
    }
}
