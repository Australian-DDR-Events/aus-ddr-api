using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class AddDancerRequest
    {
        public const string Route = "/dancers/add";
        public static string BuildRoute(Guid id) => Route.Replace("{Id:guid}", id.ToString());

        [FromQuery]
        public string DdrName { get; set; } = string.Empty;
        [FromQuery]
        public string DdrCode { get; set; } = string.Empty;
        [FromQuery]
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        [FromQuery]
        public string State { get; set; } = string.Empty;

    }
}