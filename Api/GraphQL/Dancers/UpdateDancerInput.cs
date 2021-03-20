using System;
using AusDdrApi.Entities;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Dancers
{
    public record UpdateDancerInput(
        [ID(nameof(Dancer))] Guid DancerId,
        string DdrName, 
        string DdrCode, 
        string PrimaryMachineLocation, 
        string State);
    
}