using System;
using AusDdrApi.Entities;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Badges
{
    public record AddBadgeInput(
        string Name, 
        string Description, 
        [ID(nameof(Event))] Guid EventId, 
        IFile BadgeImage);
}