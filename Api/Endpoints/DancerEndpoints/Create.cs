using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Ardalis.Result;
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
        if (dancerResult.IsSuccess) return Accepted();
        if (dancerResult.Status == ResultStatus.Error) return Conflict();
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}