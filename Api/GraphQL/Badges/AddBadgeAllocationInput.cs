using System;
using AusDdrApi.Entities;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Badges
{
    public record AddBadgeAllocationInput(
        [ID(nameof(Dancer))] Guid DancerId,
        [ID(nameof(Badge))] Guid BadgeId);
}