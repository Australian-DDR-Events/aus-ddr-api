using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Avatar_Set : EndpointWithoutResponse<SetAvatarForDancerByTokenRequest>
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;

    public Avatar_Set(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }

    [HttpPost(SetAvatarForDancerByTokenRequest.Route)]
    [SwaggerOperation(
        Summary = "Set an avatar for a dancer",
        Description = "Set the avatar for the currently logged in dancer",
        OperationId = "Dancers.Avatar.Add",
        Tags = new[] { "Dancers", "Avatar" })
    ]
    [Authorize]
    public override async Task<ActionResult> HandleAsync([FromForm] SetAvatarForDancerByTokenRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var userInfo = await _identity.GetUserInfo(HttpContext.Request.Headers["authorization"].First());
        if (request.Image == null)
        {
            return new BadRequestResult();
        }
        var result = await _dancerService.SetAvatarForDancerByAuthId(userInfo.UserId, request.Image.OpenReadStream(), cancellationToken);
        return this.ConvertToActionResult(result);
    }
}