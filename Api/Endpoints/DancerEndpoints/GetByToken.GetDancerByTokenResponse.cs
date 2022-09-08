using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancerByTokenResponse
{
    private GetDancerByTokenResponse(Guid id, string name, string code, string primaryLocation, string state, string profilePictureUrl)
    {
        Id = id;
        Name = name;
        Code = code;
        PrimaryLocation = primaryLocation;
        State = state;
        ProfilePictureUrl = profilePictureUrl;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public string Code { get; set; }

    public string PrimaryLocation { get; set; }

    public string State { get; set; }

    public string ProfilePictureUrl { get; set; }

    public ICollection<Roles> UserRoles { get; set; } = new List<Roles>();

    public static GetDancerByTokenResponse Convert(Dancer d) =>
        new GetDancerByTokenResponse(
            d.Id,
            d.DdrName,
            d.DdrCode,
            d.PrimaryMachineLocation,
            d.State, 
            $"/profile/picture/{d.Id}.png?time={d.ProfilePictureTimestamp?.GetHashCode()}"
        );

    public enum Roles
    {
        ADMIN
    }
}
