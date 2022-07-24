using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class BadgeGenerator
{
    public static Badge CreateBadge() => new Badge
    {
        Id = Guid.NewGuid()
    };
}