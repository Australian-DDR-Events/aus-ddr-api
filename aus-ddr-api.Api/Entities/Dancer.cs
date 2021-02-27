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
    }
}