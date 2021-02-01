using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Dancer
    {
        public Guid Id { get; set; }
        public string AuthenticationId { get; set; }
        public string DdrName { get; set; }
        public string DdrCode { get; set; }
        public string PrimaryMachineLocation { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<Score> Scores { get; set; }
    }
}