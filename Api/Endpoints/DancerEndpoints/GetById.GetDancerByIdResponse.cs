using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    
        public string Code { get; set; } = string.Empty;

        public string PrimaryLocation { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}