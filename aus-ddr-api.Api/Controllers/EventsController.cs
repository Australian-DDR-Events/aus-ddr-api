using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ICoreData _coreService;
        private readonly IEvent _eventService;

        public EventsController(
            ILogger<EventsController> logger,
            ICoreData coreService,
            IEvent eventService)
        {
            _logger = logger;
            _coreService = coreService;
            _eventService = eventService;
        }

        [HttpGet]
        public IEnumerable<EventResponse> GetActive()
        {
            return _eventService.GetActiveEvents().Select(EventResponse.FromEntity).ToList();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<EventResponse>> Post(EventRequest eventRequest)
        {
            var eventEntity = eventRequest.ToEntity();
            var newEvent = await _eventService.Add(eventEntity);
            if (newEvent == null)
            {
                return BadRequest();
            }

            await _coreService.SaveChanges();

            return Ok(EventResponse.FromEntity(newEvent));
        }
    }
}