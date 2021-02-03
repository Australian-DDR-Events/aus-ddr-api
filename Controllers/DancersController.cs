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

        [HttpGet("{authId}")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Dancer>> Post(DancerRequest dancerRequest)
        {
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == HttpContext.GetUserId());
            if (existingDancer != null)
            {
                return Conflict();
            }
            var dancer = dancerRequest.ToDancer();
            dancer.AuthenticationId = HttpContext.GetUserId();
            var newDancer = await _context.Dancers.AddAsync(dancer);
            await _context.SaveChangesAsync();
            return newDancer.Entity;
        }

        [HttpPut("{dancerId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Dancer>> Put(Guid dancerId, DancerRequest dancerRequest)
        {
            var existingDancer = await _context.Dancers.FindAsync(dancerId);
            if (existingDancer == null)
            {
                return NotFound();
            }
            if (existingDancer.AuthenticationId != HttpContext.GetUserId())
            {
                return Unauthorized();
            }
            var dancer = dancerRequest.ToDancer();
            dancer.Id = dancerId;
            dancer.AuthenticationId = HttpContext.GetUserId();
            var newDancer = _context.Dancers.Update(dancer);
            await _context.SaveChangesAsync();
            return Ok(newDancer.Entity);
        }
    }
}