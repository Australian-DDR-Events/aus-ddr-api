using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Badges
{
    public class AddBadgePayload : Payload
    {
        public AddBadgePayload(Badge badge)
        {
            Badge = badge;
        }
        
        public AddBadgePayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}

        public Badge? Badge { get; }
    }
}