using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Badges
{
    public class AddBadgeAllocationPayload : Payload
    {
        public AddBadgeAllocationPayload() : base()
        {
        }

        public AddBadgeAllocationPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
    }
}