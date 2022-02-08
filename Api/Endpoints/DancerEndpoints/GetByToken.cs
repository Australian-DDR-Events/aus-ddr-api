using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetByToken : EndpointWithResponse<GetDancerByTokenResponse, Dancer>
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
    public override async Task<ActionResult<GetDancerByTokenResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userInfo = await _identity.GetUserInfo(HttpContext.Request.Headers["authorization"].First().Split(" ")[1]);
        var dancerResult = await _dancerService.MigrateDancer(new MigrateDancerRequestModel
        {
            AuthId = userInfo.UserId,
            LegacyId = userInfo.LegacyId,
        }, cancellationToken);
        return this.ConvertToActionResult(dancerResult);
    }

    public override GetDancerByTokenResponse Convert(Dancer u) => new() {Id = u.Id, Name = u.DdrName, Code = u.DdrCode, State = u.State, PrimaryLocation = u.PrimaryMachineLocation};
}
