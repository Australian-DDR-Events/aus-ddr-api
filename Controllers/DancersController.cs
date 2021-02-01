using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class DancersController : ControllerBase
    {
        private readonly ILogger<DancersController> _logger;
        private DatabaseContext _context;

        public DancersController(ILogger<DancersController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DancerResponse> Get()
        {
            return _context.Dancers.Select(DancerResponse.FromDancer).ToArray();
        }

        [HttpGet]
        [Route("~/dancers/{authId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DancerResponse> GetDancer(string authId)
        {
            var dancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authId);
            if (dancer == null)
            {
                return NotFound();
            }

            return DancerResponse.FromDancer(dancer);
        }

        [HttpPost]
        [Authorize]
        public Dancer Post(DancerRequest dancerRequest)
        {
            dancerRequest.AuthenticationId = HttpContext.GetUserId();
            var newDancer = _context.Dancers.Add(dancerRequest.ToDancer());
            _context.SaveChanges();
            return newDancer.Entity;
        }
    }
}