using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DancerResponse
    {
        public Guid Id { get; set; }
        public string AuthenticationId { get; set; } = string.Empty;
        public string DdrName { get; set; } = string.Empty;
        public string DdrCode { get; set; } = string.Empty;
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;

        public static DancerResponse FromEntity(Dancer dancer) => new DancerResponse
        {
            Id = dancer.Id,
            AuthenticationId = dancer.AuthenticationId,
            DdrName = dancer.DdrName,
            DdrCode = dancer.DdrCode,
            PrimaryMachineLocation = dancer.PrimaryMachineLocation,
            State = dancer.State,
            ProfilePictureUrl = $"/profile/picture/{dancer.AuthenticationId}.png"
        };
    }
}