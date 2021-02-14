using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Requests
{
    public class DancerRequest : IValidatableObject
    {
        [Required]
        public string DdrName { get; set; }
        [Required]
        public string DdrCode { get; set; }
        [Required]
        public string PrimaryMachineLocation { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ProfilePictureUrl { get; set; }
        
        public Dancer ToEntity() => new Dancer
        {
            DdrName = DdrName,
            DdrCode = DdrCode,
            PrimaryMachineLocation = PrimaryMachineLocation,
            State = State,
        };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!new[] {"vic", "act", "nsw", "sa", "nt", "tas", "qld", "wa"}.Contains(State))
                yield return new ValidationResult("Invalid state");
            
            if (!int.TryParse(DdrCode, out _))
                yield return new ValidationResult("Invalid DDR Code");
        }
    }
}