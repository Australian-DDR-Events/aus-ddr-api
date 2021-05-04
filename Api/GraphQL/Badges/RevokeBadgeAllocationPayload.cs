using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Badges
{
    public class RevokeBadgeAllocationPayload : Payload
    {
        public RevokeBadgeAllocationPayload() : base()
        {
        }

        public RevokeBadgeAllocationPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
    }
}