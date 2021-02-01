using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
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
        
        [HttpPost]
        public Dancer Post(DancerRequest dancerRequest)
        {
            var newDancer = _context.Dancers.Add(dancerRequest.ToDancer());
            _context.SaveChanges();
            return newDancer.Entity;
        }
    }
}