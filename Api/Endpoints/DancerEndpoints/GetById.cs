using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetById : BaseAsyncEndpoint
        .WithRequest<GetDancerByIdRequest>
        .WithResponse<GetDancerByIdResponse>
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
        public override async Task<ActionResult<GetDancerByIdResponse>> HandleAsync([FromRoute] GetDancerByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return Ok(await _dancerService.GetDancerByIdAsync(request.Id, cancellationToken));
        }
    }
}