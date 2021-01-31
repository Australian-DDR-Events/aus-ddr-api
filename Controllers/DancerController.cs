using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Models;
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
        public IEnumerable<Dancer> Get()
        {
            return _context.Dancers.AsQueryable().ToArray();
        }
        
        [HttpPost]
        public Dancer Post(Dancer dancer)
        {
            dancer.Id = Guid.Empty;
            var newDancer = _context.Dancers.Add(dancer);
            _context.SaveChanges();
            return newDancer.Entity;
        }
    }
}