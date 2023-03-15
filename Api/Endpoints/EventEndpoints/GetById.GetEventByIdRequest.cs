using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.EventEndpoints;

public class GetEventByIdRequest
{  
    [FromRoute]
    public Guid Id { get; set; }
}