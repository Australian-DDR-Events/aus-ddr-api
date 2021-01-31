using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Models;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DancerController : ControllerBase
    {
        private readonly ILogger<DancerController> _logger;
        private DatabaseContext _context;

        public DancerController(ILogger<DancerController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IEnumerable<DancerResponse> Get()
        {
            return _context.Dancers.Select(DancerResponse.FromDancer).ToArray();
        }
        
        [HttpPost]
        public Dancer Post(DancerRequest dancerRequest)
        {
            var newDancer = _context.Dancers.Add(dancerRequest.ToDancer());
            _context.SaveChanges();
            return newDancer.Entity;
        }
    }
}