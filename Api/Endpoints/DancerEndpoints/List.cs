using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class List : EndpointWithResponse<GetDancersRequest, GetDancersResponse, IList<Dancer>>
    {
        private readonly IDancerService _dancerService;

        public List(IDancerService dancerService)
        {
            _dancerService = dancerService;
        }
        
        [HttpGet(GetDancersRequest.Route)]
        [SwaggerOperation(
            Summary = "Gets a single Dancer",
            Description = "Gets a single Dancer by Id",
            OperationId = "Dancers.GetById",
            Tags = new[] { "Dancers" })
        ]
        public override async Task<ActionResult<GetDancersResponse>> HandleAsync([FromQuery] GetDancersRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var dancersResult = await _dancerService.GetDancersAsync(request.Page.GetValueOrDefault(0), request.Limit.GetValueOrDefault(20), cancellationToken);
            return this.ConvertToActionResult(dancersResult);
        }

        public override GetDancersResponse Convert(IList<Dancer> u)
        {
            return new GetDancersResponse()
            {
                Dancers = u.Select(d => new DancerRecord(d.Id, d.DdrName)).ToList()
            };
        }
    }
}