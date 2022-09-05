using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Connections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.ConnectionEndpoints;

[ApiController]
public class Discord : ControllerBase
{
    private readonly IConnectionService<DiscordConnectionRequestModel> _connectionService;
    private readonly IIdentity<string> _identity;

    public Discord(IConnectionService<DiscordConnectionRequestModel> connectionService, IIdentity<string> identity)
    {
        _connectionService = connectionService;
        _identity = identity;
    }

    [Authorize]
    [HttpPost("/connections/discord")]
    public async Task<ActionResult<bool>> HandleAsync([FromHeader] string authorization, [FromQuery] string code, CancellationToken cancellationToken = new())
    {
        var userInfo = await _identity.GetUserInfo(authorization);
        var result = await _connectionService.CreateConnection(userInfo.UserId,
            new DiscordConnectionRequestModel() {Code = code}, cancellationToken);
        return Ok(result);
    }
}