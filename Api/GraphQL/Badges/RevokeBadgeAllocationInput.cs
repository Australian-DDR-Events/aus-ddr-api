using System;
using AusDdrApi.Entities;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Badges
{
    public record RevokeBadgeAllocationInput(
        [ID(nameof(Dancer))] Guid DancerId,
        [ID(nameof(Badge))] Guid BadgeId);
}