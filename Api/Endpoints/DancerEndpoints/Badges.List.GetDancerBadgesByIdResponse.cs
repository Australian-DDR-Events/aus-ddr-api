using System;
using Application.Core.Models.Dancer;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancerBadgesByIdResponse
{
    private GetDancerBadgesByIdResponse(Guid id, string name, string description, string eventName)
    {
        Id = id;
        Name = name;
        Description = description;
        EventName = eventName;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string? EventName { get; }

    public static GetDancerBadgesByIdResponse Convert(GetDancerBadgesResponseModel badge) =>
        new GetDancerBadgesByIdResponse(badge.Id, badge.Name, badge.Description, badge.EventName);
}
