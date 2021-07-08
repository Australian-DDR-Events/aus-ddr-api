using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using AusDdrApi.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class AddDancer : EndpointWithResponse<AddDancerRequest, AddDancerResponse, Dancer>
    {
        private readonly IDancerService _dancerService;
        private readonly IAuthorization _authorizationService;

        public AddDancer(IDancerService dancerService, IAuthorization authorizationService)
        {
            _dancerService = dancerService;
            _authorizationService = authorizationService;
        }
        
        [HttpPut(AddDancerRequest.Route)]
        [Authorize]
        [SwaggerOperation(
            Summary = "Adds a single Dancer",
            Description = "Adds a single Dancer",
            OperationId = "Dancers.AddDancer",
            Tags = new[] { "Dancers" })
        ]
        public override async Task<ActionResult<AddDancerResponse>> HandleAsync([FromRoute] [FromQuery] AddDancerRequest request, CancellationToken cancellationToken = new())
        {
            var authId = _authorizationService.GetUserId();
            if (authId == null)
                return Unauthorized();

            var existingDancer = _dancerService.GetDancerByAuthIdAsync(authId, cancellationToken);
            if (existingDancer != null)
                return Conflict();

            var id = Guid.NewGuid();

            await _dancerService.AddNewDancerAsync(id, request.DdrName, request.DdrCode, authId, request.PrimaryMachineLocation, request.State, cancellationToken);

            var dancerResult = await _dancerService.GetDancerByIdAsync(id, cancellationToken);
            return this.ConvertToActionResult(dancerResult);
        }
        
        public override AddDancerResponse Convert(Dancer u) => new() {Id = u.Id, Name = u.DdrName};
    }
}