using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using EventEntity = AusDdrApi.Entities.Event;

namespace AusDdrApi.Services.Event
{
    public class DbEvent : IEvent
    {   
        private DatabaseContext _context;

        public DbEvent(DatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<EventEntity> GetAll()
        {
            return _context.Events.ToList();
        }

        public EventEntity? Get(Guid eventId)
        {
            return _context.Events.Find(eventId);
        }

        public IEnumerable<EventEntity> GetActiveEvents()
        {
            return _context
                .Events
                .Where(e => e.StartDate <= DateTime.Now
                            && e.EndDate >= DateTime.Now)
                .ToList();
        }

        public async Task<EventEntity> Add(EventEntity eEvent)
        {
            var newEvent = await _context.Events.AddAsync(eEvent);
            return newEvent.Entity;
        }

        public EventEntity? Update(EventEntity eEvent)
        {
            var currentEvent = Get(eEvent.Id);
            if (currentEvent == null) return null;
            _context.Entry(currentEvent).CurrentValues.SetValues(eEvent);
            return currentEvent;
        }

        public void Delete(Guid eventId)
        {
            var currentEvent = Get(eventId);
            if (currentEvent != null) _context.Events.Remove(currentEvent);
        }
    }
}