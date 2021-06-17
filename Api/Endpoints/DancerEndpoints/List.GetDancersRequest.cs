using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancersRequest
    {
        public const string Route = "/dancers";
        
        [FromQuery]
        public int? Page { get; set; } = 0;
        
        [FromQuery]
        [Range(1, 100, ErrorMessage = "Cannot request more than 100 dancers per request")]
        public int? Limit { get; set; }
    }
}