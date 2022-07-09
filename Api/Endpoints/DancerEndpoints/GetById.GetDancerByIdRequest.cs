using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdRequest
    {
        [FromRoute]
        public Guid Id { get; set; }
    }
}