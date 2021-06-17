using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdRequest
    {
        public const string Route = "/dancers/{Id:guid}";
        public static string BuildRoute(Guid id) => Route.Replace("{Id:guid}", id.ToString());
        
        [FromRoute]
        public Guid Id { get; set; }
    }
}