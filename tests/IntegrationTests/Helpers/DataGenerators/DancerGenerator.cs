using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class DancerGenerator
{
    public static Dancer CreateDancer() => new()
    {
        Id = Guid.NewGuid(),
        AuthenticationId = Guid.NewGuid().ToString()
    };

    public static Dancer CreateDancer(string name) => new()
    {
        Id = Guid.NewGuid(),
        AuthenticationId = Guid.NewGuid().ToString(),
        DdrName = name
    };
}