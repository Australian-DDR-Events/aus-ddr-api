using System;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancersResponse
{
    private GetDancersResponse(Guid id, string ddrName, string profilePictureUrl)
    {
        Id = id;
        DdrName = ddrName;
        ProfilePictureUrl = profilePictureUrl;
    }
    
    public Guid Id { get; }
    public string DdrName { get; }
    public string ProfilePictureUrl { get; }

    public static GetDancersResponse Convert(Dancer d) =>
        new(
            d.Id,
            d.DdrName,
            $"/profile/picture/{d.Id}.png?time={d.ProfilePictureTimestamp?.GetHashCode()}"
        );
}
