using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class UpdateDancerByAuthIdRequest
    {
             public string DdrName { get; set; } = string.Empty;
             public string DdrCode { get; set; } = string.Empty;
             public string PrimaryMachineLocation { get; set; } = string.Empty;
             public string State { get; set; } = string.Empty;

    }
}