using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Dancers
{
    public class AddDancerPayload : Payload
    {
        public AddDancerPayload(Dancer dancer) {}
        public AddDancerPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
        
        public Dancer? Dancer { get; }
    }
}