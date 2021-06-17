using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetById : EndpointWithResponse<GetDancerByIdRequest, GetDancerByIdResponse, Dancer>
    {
        private readonly IDancerService _dancerService;

        public GetById(IDancerService dancerService)
        {
            _dancerService = dancerService;
        }
        
        [HttpGet(GetDancerByIdRequest.Route)]
        [SwaggerOperation(
            Summary = "Gets a single Dancer",
            Description = "Gets a single Dancer by Id",
            OperationId = "Dancers.GetById",
            Tags = new[] { "Dancers" })
        ]
        public override async Task<ActionResult<GetDancerByIdResponse>> HandleAsync([FromRoute] [FromQuery] GetDancerByIdRequest request, CancellationToken cancellationToken = new())
        {
            var dancerResult = await _dancerService.GetDancerByIdAsync(request.Id, cancellationToken);
            return this.ConvertToActionResult(dancerResult);
        }
        
        public override GetDancerByIdResponse Convert(Dancer u) => new() {Id = u.Id, Name = u.DdrName};
    }
}