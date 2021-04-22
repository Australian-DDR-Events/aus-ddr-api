using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;

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
        
        public DateTime? ProfilePictureTimestamp { get; set; }
        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<BadgeType>>>))]
        public virtual ICollection<Badge> Badges { get; set; } = new HashSet<Badge>();
        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ScoreType>>>))]
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        [NotMapped]
        public string ProfilePictureUrl =>
            new StringBuilder()
                .Append("/profile/picture/")
                .Append($"{AuthenticationId}")
                .Append(ProfilePictureTimestamp != null
                    ? $".{(ProfilePictureTimestamp.Value.Ticks - 621355968000000000) / 10000000}"
                    : "")
                .Append(".png")
                .ToString();
                
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
                State == comparator.State &&
                ProfilePictureTimestamp == comparator.ProfilePictureTimestamp);
        }

        public override int GetHashCode()
        {
            return (Id, AuthenticationId, DdrName, DdrCode, PrimaryMachineLocation, State, ProfilePictureTimestamp).GetHashCode();
        }
    }
}