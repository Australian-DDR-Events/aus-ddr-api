using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Dancers
{
    public class UpdateDancerPayload : Payload
    {
        public UpdateDancerPayload(Dancer dancer)
        {
            Dancer = dancer;
        }
        
        public UpdateDancerPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
        
        public Dancer? Dancer { get; }
    }
}