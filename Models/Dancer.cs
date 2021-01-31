using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AusDdrApi.Models
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
    }
}