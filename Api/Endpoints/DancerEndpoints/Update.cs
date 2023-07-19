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

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    [ApiController]
    public class Update : ControllerBase
    {
        private readonly IDancerService _dancerService;
        private readonly IIdentity<string> _identity;

        public Update(IDancerService dancerService, IIdentity<string> identity)
        {
            _dancerService = dancerService;
            _identity = identity;
        }
        
        [HttpPut("/dancers")]
        [SwaggerOperation(
            Summary = "Updates a dancer by AuthId",
            Description = "Attempts to update a given dancer using both the auth id, or the legacy auth id",
            OperationId = "Dancers.UpdateDancerByAuthIdRequest",
            Tags = new[] { "Dancers" })
        ]
        [Authorize]
        public async Task<ActionResult> HandleAsync(
            [FromBody] UpdateDancerByAuthIdRequest request,
            [FromHeader] string authorization,
            CancellationToken cancellationToken = new())
        {
            var userInfo = await _identity.GetUserInfo(authorization);

            var requestModel = new UpdateDancerRequestModel
            {
                AuthId = userInfo.UserId,
                DdrCode = request.DdrCode,
                DdrName = request.DdrName,
                PrimaryMachineLocation = request.PrimaryMachineLocation,
                State = request.State
            };

            var dancerResult = await _dancerService.UpdateDancerAsync(requestModel, cancellationToken);
            return dancerResult.ResultCode switch
            {
                ResultCode.Ok => Accepted(),
                ResultCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError),
            };
        }
    }
}