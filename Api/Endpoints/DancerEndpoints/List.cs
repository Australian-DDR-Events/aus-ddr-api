using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

[ApiController]
public class List : ControllerBase
{
    private readonly IDancerService _dancerService;

    public List(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
    
    [HttpGet("/dancers")]
    [SwaggerOperation(
        Summary = "Gets a collection of dancers",
        Description = "Gets a number of dancers by paging",
        OperationId = "Dancers.List",
        Tags = new[] { "Dancers" })
    ]
    public async Task<ActionResult<GetDancersResponse>> HandleAsync([FromQuery] GetDancersRequest request, CancellationToken cancellationToken = new())
    {
        var dancersResult = await _dancerService.GetDancersAsync(request.Page.GetValueOrDefault(0), request.Limit.GetValueOrDefault(20), cancellationToken);
        return dancersResult.ResultCode switch
        {
            ResultCode.Ok => Ok(dancersResult.Value.Value.Select(GetDancersResponse.Convert)),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}
