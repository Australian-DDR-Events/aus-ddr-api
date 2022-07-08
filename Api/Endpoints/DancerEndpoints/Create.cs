using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Create : EndpointWithoutResponse<CreateDancerByAuthIdRequest>
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;

    public Create(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }
        
    [HttpPost(CreateDancerByAuthIdRequest.Route)]
    [SwaggerOperation(
        Summary = "Creates a dancer by AuthId",
        Description = "Attempts to create a dancer for the given user",
        OperationId = "Dancers.CreateDancerByAuthIdRequest",
        Tags = new[] { "Dancers" })
    ]
    [Authorize]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateDancerByAuthIdRequest request, CancellationToken cancellationToken = new())
    {
        var userInfo = await _identity.GetUserInfo(HttpContext.Request.Headers["authorization"].First());

        var requestModel = new CreateDancerRequestModel
        {
            AuthId = userInfo.UserId,
            DdrCode = request.DdrCode,
            DdrName = request.DdrName,
            PrimaryMachineLocation = request.PrimaryMachineLocation,
            State = request.State
        };

        var dancerResult = await _dancerService.CreateDancerAsync(requestModel, cancellationToken);
        return this.ConvertToActionResult(dancerResult, Accepted());
    }
}