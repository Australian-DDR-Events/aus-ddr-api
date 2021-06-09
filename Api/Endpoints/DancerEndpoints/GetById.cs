using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

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
        
        [HttpGet("/dancers/{Id:guid}")]
        public override async Task<ActionResult<GetDancerByIdResponse>> HandleAsync([FromRoute] GetDancerByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return Ok(_dancerService.GetDancerByIdAsync(request.Id, cancellationToken));
        }
    }
}