using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.EventEndpoints
{
    public class Create : EndpointWithResponse<CreateEventRequest, CreateEventResponse, Event>
    {
        public override Task<ActionResult<CreateEventResponse>> HandleAsync(CreateEventRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override CreateEventResponse Convert(Event u)
        {
            throw new System.NotImplementedException();
        }
    }
}