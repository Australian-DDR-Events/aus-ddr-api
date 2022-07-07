using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Ardalis.Result;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetByToken : ControllerBase
{
    private readonly IDancerService _dancerService;
    private readonly IIdentity<string> _identity;

    public GetByToken(IDancerService dancerService, IIdentity<string> identity)
    {
        _dancerService = dancerService;
        _identity = identity;
    }

    [Authorize]
    [HttpGet("/dancers/me")]
    public async Task<ActionResult<GetDancerByTokenResponse>> HandleAsync([FromHeader] string authorization, CancellationToken cancellationToken = new())
    {
        var userInfo = await _identity.GetUserInfo(authorization);
        var dancerResult = await _dancerService.MigrateDancer(new MigrateDancerRequestModel
        {
            AuthId = userInfo.UserId,
            LegacyId = userInfo.LegacyId,
        }, cancellationToken);
        if (dancerResult.IsSuccess) return Ok(GetDancerByTokenResponse.Convert(dancerResult.Value));
        if (dancerResult.Status == ResultStatus.NotFound) return NotFound();
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
