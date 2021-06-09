using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}