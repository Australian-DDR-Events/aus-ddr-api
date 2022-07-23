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

[ApiController]
public class Avatar_Set : ControllerBase
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;

    public Avatar_Set(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }

    [HttpPost("/dancers/avatar")]
    [SwaggerOperation(
        Summary = "Set an avatar for a dancer",
        Description = "Set the avatar for the currently logged in dancer",
        OperationId = "Dancers.Avatar.Add",
        Tags = new[] { "Dancers", "Avatar" })
    ]
    [Authorize]
    public async Task<ActionResult> HandleAsync([FromForm] SetAvatarForDancerByTokenRequest request, [FromHeader] string authorization,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var userInfo = await _identity.GetUserInfo(authorization);
        if (request.Image == null)
        {
            return new BadRequestResult();
        }
        var result = await _dancerService.SetAvatarForDancerByAuthId(userInfo.UserId, request.Image.OpenReadStream(), cancellationToken);
        return result ? Ok() : NotFound();
    }
}