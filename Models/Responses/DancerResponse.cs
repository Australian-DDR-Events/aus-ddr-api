using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DancerResponse
    {
        public Guid Id { get; set; }
        public string AuthenticationId { get; set; }
        public string DdrName { get; set; }
        public string DdrCode { get; set; }
        public string PrimaryMachineLocation { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }

        public static DancerResponse FromEntity(Dancer dancer) => new DancerResponse
        {
            Id = dancer.Id,
            AuthenticationId = dancer.AuthenticationId,
            DdrName = dancer.DdrName,
            DdrCode = dancer.DdrCode,
            PrimaryMachineLocation = dancer.PrimaryMachineLocation,
            State = dancer.State,
            ProfilePictureUrl = dancer.ProfilePictureUrl
        };
    }
}