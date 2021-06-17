using System.Collections.Generic;

namespace AusDdrApi.Endpoints.BadgeEndpoints
{
    public class GetBadgesResponse
    {
        public IList<BadgeRecord> Badges { get; set; } = default!;
    }
}