using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Ardalis.Result;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

[ApiController]
public class GetByToken : ControllerBase
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;

    public GetByToken(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }

    [UserAuth]
    [HttpGet("/dancers/me")]
    public async Task<ActionResult<GetDancerByTokenResponse>> HandleAsync(CancellationToken cancellationToken = new())
    {
        var cookie = HttpContext.Items["cookie"] as string;
        if (cookie == null) return Unauthorized();
        var userInfo = await _identity.GetUserInfo(cookie);
        var dancerResult = await _dancerService.MigrateDancer(new MigrateDancerRequestModel
        {
            AuthId = userInfo.UserId,
            LegacyId = userInfo.LegacyId,
        }, cancellationToken);
        if (dancerResult.IsSuccess)
        {
            var dancer = GetDancerByTokenResponse.Convert(dancerResult.Value);
            if (_identity.IsAdmin(cookie)) dancer.UserRoles.Add(GetDancerByTokenResponse.Roles.ADMIN);
            return Ok(dancer);
        }
        if (dancerResult.Status == ResultStatus.NotFound) return NotFound();
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
