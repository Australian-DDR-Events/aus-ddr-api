using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.BadgeEndpoints
{
    public class List : EndpointWithResponse<GetBadgesRequest, GetBadgesResponse, IList<Badge>>
    {
        private readonly IBadgeService _badgeService;

        public List(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }
        
        [HttpGet(GetBadgesRequest.Route)]
        [SwaggerOperation(
            Summary = "Gets a collection of Badges",
            Description = "Gets a collection of badges based on paging request",
            OperationId = "Badges.List",
            Tags = new[] { "Badges" })
        ]
        public override async Task<ActionResult<GetBadgesResponse>> HandleAsync(GetBadgesRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var badgesResult = await _badgeService.GetBadgesAsync(request.Page.GetValueOrDefault(0), request.Limit.GetValueOrDefault(20), cancellationToken);
            return this.ConvertToActionResult(badgesResult);
        }

        public override GetBadgesResponse Convert(IList<Badge> u)
        {
            return new()
            {
                Badges = u.Select(b => new BadgeRecord(b.Id, b.Name, "temp")).ToList()
            };
        }
    }
}