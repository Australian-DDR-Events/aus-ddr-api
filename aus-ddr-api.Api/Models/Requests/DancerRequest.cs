using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Requests
{
    public class DancerRequest : IValidatableObject
    {
        public string DdrName { get; set; } = string.Empty;
        public string DdrCode { get; set; } = "573";
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        public string State { get; set; } = "n/a";

        public Dancer ToEntity() => new Dancer
        {
            DdrName = DdrName,
            DdrCode = DdrCode,
            PrimaryMachineLocation = PrimaryMachineLocation,
            State = State,
        };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!new[] {"n/a", "vic", "act", "nsw", "sa", "nt", "tas", "qld", "wa"}.Contains(State))
                yield return new ValidationResult("Invalid state");
            
            if (!int.TryParse(DdrCode, out _))
                yield return new ValidationResult("Invalid DDR Code");
        }
    }
}