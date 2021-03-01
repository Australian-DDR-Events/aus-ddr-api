using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Dancer
    {
        public Guid Id { get; set; }
        public string AuthenticationId { get; set; } = string.Empty;
        public string DdrName { get; set; } = string.Empty;
        public string DdrCode { get; set; } = string.Empty;
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        
        public virtual ICollection<Badge> Badges { get; set; } = new HashSet<Badge>();
                
        public override bool Equals(object? comparator)
        {
            var comparatorAsSong = comparator as Dancer;
            if (comparatorAsSong == null) return false;
            return Equals(comparatorAsSong);
        }

        public bool Equals(Dancer comparator)
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