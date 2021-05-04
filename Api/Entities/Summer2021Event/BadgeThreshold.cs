using System;
using System.ComponentModel.DataAnnotations;

namespace AusDdrApi.Entities
{
    public class BadgeThreshold
    {
        [Key]
        public Guid BadgeId { get; set; }
        public Badge? Badge { get; set; }
        public int RequiredPoints { get; set; }
    }
}