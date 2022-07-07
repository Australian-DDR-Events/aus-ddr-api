using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class EventGenerator
{
    public static Event CreateEvent() => new Event
    {
        Id = Guid.NewGuid(),
        StartDate = DateTime.Now
    };
}