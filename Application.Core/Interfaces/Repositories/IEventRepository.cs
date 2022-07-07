using System;
using System.Collections.Generic;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Event? GetEventWithTopScore(Guid eventId);
    IEnumerable<Event> GetEvents();
    IEnumerable<Course> GetEventCourses(Guid eventId);
    IEnumerable<SongDifficulty> GetEventSongs(Guid eventId);
}