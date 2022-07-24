using System;
using Application.Core.Models.Badge;

namespace AusDdrApi.Endpoints.BadgeEndpoints;

public class GetBadgesResponse
{
    private GetBadgesResponse(Guid id, string name, string description, string? eventName)
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

    public static GetBadgesResponse Convert(GetBadgesResponseModel getBadgesResponseModel) => new GetBadgesResponse(
        getBadgesResponseModel.Id,
        getBadgesResponseModel.Name,
        getBadgesResponseModel.Description, 
        getBadgesResponseModel.EventName
    );
}