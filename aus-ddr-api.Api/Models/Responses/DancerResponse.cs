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
        public override bool Equals(object? comparator)
        {
            var comparatorAsSongResponse = comparator as DancerResponse;
            if (comparatorAsSongResponse == null) return false;
            return Equals(comparatorAsSongResponse);
        }

        public bool Equals(DancerResponse comparator)
        {
            return (
                Id == comparator.Id &&
                AuthenticationId == comparator.AuthenticationId &&
                DdrName == comparator.DdrName &&
                DdrCode == comparator.DdrCode &&
                PrimaryMachineLocation == comparator.PrimaryMachineLocation &&
                State == comparator.State);
        }

        public override int GetHashCode()
        {
            return (Id, AuthenticationId, DdrName, DdrCode, PrimaryMachineLocation, State).GetHashCode();
        }
    }
}