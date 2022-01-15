using System;

namespace AusDdrApi.Endpoints.BadgeEndpoints
{
    public class CreateBadgeRequest
    {
        public const string Route = "/badges";

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Threshold { get; set; } = null;
 
        public Guid EventId { get; set; }
    }
}