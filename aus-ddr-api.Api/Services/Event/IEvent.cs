using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventEntity = AusDdrApi.Entities.Event;

namespace AusDdrApi.Services.Event
{
    public interface IEvent
    {
        public IEnumerable<EventEntity> GetAll();
        public EventEntity? Get(Guid eventId);
        public IEnumerable<EventEntity> GetActiveEvents();

        public Task<EventEntity> Add(EventEntity eEvent);
        public EventEntity? Update(EventEntity eEvent);
        public void Delete(Guid eventId);
    }
}