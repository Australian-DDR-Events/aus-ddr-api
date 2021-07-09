using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
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
            Summary = "Gets a collection of dancers",
            Description = "Gets a number of dancers by paging",
            OperationId = "Dancers.List",
            Tags = new[] { "Dancers" })
        ]
        public override async Task<ActionResult<GetDancersResponse>> HandleAsync([FromQuery] GetDancersRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var dancersResult = await _dancerService.GetDancersAsync(request.Page.GetValueOrDefault(0), request.Limit.GetValueOrDefault(20), cancellationToken);
            return this.ConvertToActionResult(dancersResult);
        }

        public override GetDancersResponse Convert(IList<Dancer> u)
        {
            return new()
            {
                Dancers = u.Select(d => new DancerRecord(
                    d.Id, d.DdrName, d.DdrCode, d.PrimaryMachineLocation, d.State, $"/profile/picture/{d.Id}.png?time={d.ProfilePictureTimestamp?.Ticks}"
                    )).ToList()
            };
        }
    }
}