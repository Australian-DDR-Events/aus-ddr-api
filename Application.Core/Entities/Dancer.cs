using System;
using System.Collections.Generic;
using Application.Core.Interfaces;

namespace Application.Core.Entities
{
    public class Dancer : BaseEntity, IAggregateRoot
    {
        public string AuthenticationId { get; set; } = string.Empty;
        public string DdrName { get; set; } = string.Empty;
        public string DdrCode { get; set; } = string.Empty;
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        
        public DateTime? ProfilePictureTimestamp { get; set; }
        
        public virtual ICollection<Badge> Badges { get; set; } = default!;
        public ICollection<Score> Scores { get; set; } = default!;
    }
}