using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
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
            return dancerResult.ResultCode switch
            {
                ResultCode.Ok => Ok(GetDancerByIdResponse.Convert(dancerResult.Value.Value)),
                ResultCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError),
            };
        }
    }
}