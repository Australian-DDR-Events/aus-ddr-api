using System.Collections.Generic;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancersResponse
    {
        public IList<DancerRecord> Dancers { get; set; }
    }
}