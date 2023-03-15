using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancersResponse
{
    private GetDancersResponse(Guid id, string ddrName, IDictionary<string, string> profilePictureUrls)
    {
        Id = id;
        DdrName = ddrName;
        ProfilePictureUrls = profilePictureUrls;
    }
    
    public Guid Id { get; }
    public string DdrName { get; }
    public IDictionary<string, string> ProfilePictureUrls { get; }

    public static GetDancersResponse Convert(Dancer d) =>
        new(
            d.Id,
            d.DdrName,
            ProfilePictureTypes.ToDictionary(type => type, type => $"/profile/avatar/{d.Id}.{type}.png?time={d.ProfilePictureTimestamp?.GetHashCode()}")
        );

    private static readonly IEnumerable<string> ProfilePictureTypes = new List<string>() {"128", "256"};
}
