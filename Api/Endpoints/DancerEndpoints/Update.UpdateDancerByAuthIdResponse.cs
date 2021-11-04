using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class UpdateDancerByAuthIdResponse
    {   
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
    }
}