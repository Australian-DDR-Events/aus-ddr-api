using System;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.EventEndpoints;

public class ListEventResponse
{
    private ListEventResponse(Guid eventId, string name, string description, DateTime startDate, DateTime endDate)
    {
        EventId = eventId;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public Guid EventId { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public static ListEventResponse Convert(Event e) =>
        new(e.Id, e.Name, e.Description, e.StartDate, e.EndDate);
}
