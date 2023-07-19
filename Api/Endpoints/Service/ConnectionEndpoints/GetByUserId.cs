using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.Service.ConnectionEndpoints;

[ApiController]
public class GetByUserId : ControllerBase
{
    [Authorize]
    [ServiceAuth]
    [HttpGet("/service/test1")]
    public ActionResult EndpointNoScope()
    {
        return Ok();
    }
    
    [Authorize]
    [ServiceAuth(Scope = "connections:read")]
    [HttpGet("/service/test2")]
    public ActionResult EndpointScope()
    {
        return Ok();
    }
    
    [Authorize]
    [ServiceAuth(Scope = "connections:delete")]
    [HttpGet("/service/test3")]
    public ActionResult EndpointDeleteScope()
    {
        return Ok();
    }
}