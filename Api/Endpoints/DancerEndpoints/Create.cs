using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Application.Core.Models.Dancer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

[ApiController]
public class Create : ControllerBase
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;
    public Create(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }
        
    [HttpPost("/dancers")]
    [SwaggerOperation(
        Summary = "Creates a dancer by AuthId",
        Description = "Attempts to create a dancer for the given user",
        OperationId = "Dancers.CreateDancerByAuthIdRequest",
        Tags = new[] { "Dancers" })
    ]
    [Authorize]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateDancerByAuthIdRequest request,
        [FromHeader] string authorization,
        CancellationToken cancellationToken = new()
    )
    {
        var userInfo = await _identity.GetUserInfo(authorization);

        var requestModel = new CreateDancerRequestModel
        {
            AuthId = userInfo.UserId,
            DdrCode = request.DdrCode,
            DdrName = request.DdrName,
            PrimaryMachineLocation = request.PrimaryMachineLocation,
            State = request.State
        };

        var dancerResult = await _dancerService.CreateDancerAsync(requestModel, cancellationToken);

        return dancerResult.ResultCode switch
        {
            ResultCode.Ok => Accepted(),
            ResultCode.Conflict => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}
