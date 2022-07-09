using System;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdResponse
    {
        private GetDancerByIdResponse(Guid id, string name, string code, string primaryLocation, string state, string profilePictureUrl)
        {
            Id = id;
            Name = name;
            Code = code;
            PrimaryLocation = primaryLocation;
            State = state;
            ProfilePictureUrl = profilePictureUrl;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    
        public string Code { get; set; } = string.Empty;

        public string PrimaryLocation { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; }

        public static GetDancerByIdResponse Convert(Dancer d) =>
            new(
                d.Id,
                d.DdrName,
                d.DdrCode,
                d.PrimaryMachineLocation,
                d.State, 
                $"/profile/picture/{d.Id}.png?time={d.ProfilePictureTimestamp?.GetHashCode()}"
            );
    }
}