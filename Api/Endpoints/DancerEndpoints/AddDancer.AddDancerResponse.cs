using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class AddDancerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
    }
}