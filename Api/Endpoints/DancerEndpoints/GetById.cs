using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    [ApiController]
    public class GetById : ControllerBase
    {
        private readonly IDancerService _dancerService;

        public GetById(IDancerService dancerService)
        {
            _dancerService = dancerService;
        }
        
        [HttpGet("/dancers/{Id}")]
        [SwaggerOperation(
            Summary = "Gets a single Dancer",
            Description = "Gets a single Dancer by Id",
            OperationId = "Dancers.GetById",
            Tags = new[] { "Dancers" })
        ]
        public async Task<ActionResult<GetDancerByIdResponse>> HandleAsync([FromRoute] GetDancerByIdRequest request, CancellationToken cancellationToken = new())
        {
            var dancerResult = _dancerService.GetDancerById(request.Id);
            if (dancerResult.IsSuccess) return Ok(GetDancerByIdResponse.Convert(dancerResult.Value));
            if (dancerResult.Status == ResultStatus.NotFound) return NotFound();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}