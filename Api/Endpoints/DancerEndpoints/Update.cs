using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class Update : EndpointWithResponse<UpdateDancerByAuthIdRequest, UpdateDancerByAuthIdResponse, Dancer>
    {
        private readonly IDancerService _dancerService;
        private readonly IIdentity<string> _identity;

        public Update(IDancerService dancerService, IIdentity<string> identity)
        {
            _dancerService = dancerService;
            _identity = identity;
        }
        
        [HttpPost(UpdateDancerByAuthIdRequest.Route)]
        [SwaggerOperation(
            Summary = "Updates a dancer by AuthId",
            Description = "Attempts to update a given dancer using both the auth id, or the legacy auth id",
            OperationId = "Dancers.UpdateDancerByAuthIdRequest",
            Tags = new[] { "Dancers" })
        ]
        [Authorize]
        public override async Task<ActionResult<UpdateDancerByAuthIdResponse>> HandleAsync([FromBody] UpdateDancerByAuthIdRequest request, CancellationToken cancellationToken = new())
        {
            var a = await _identity.GetUserInfo(HttpContext.Request.Headers["authorization"].First().Split(" ")[1]);
            //var dancerResult = await _dancerService.GetByIdAsync(request.Id, cancellationToken);
            //return this.ConvertToActionResult(dancerResult);
            return Accepted();
        }
        
        public override UpdateDancerByAuthIdResponse Convert(Dancer u) => new() {Id = u.Id, Name = u.DdrName};
    }
}