using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Postgres database collection")]
public class DancerRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly DancerRepository _dancerRepository;

    public DancerRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dancerRepository = new DancerRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    #region GetDancers

    [Fact(DisplayName = "Limits to N entries")]
    public void LimitToNEntries()
    {
        var dancers = new List<Dancer>
            {DancerGenerator.CreateDancer(), DancerGenerator.CreateDancer(), DancerGenerator.CreateDancer()};
        dancers.ForEach(d => _fixture._context.Dancers.Add(d));
        _fixture._context.SaveChanges();

        var result = _dancerRepository.GetDancers(0, 2);
        
        Assert.Equal(2, result.Count());
    }

    [Fact(DisplayName = "Skips N entries")]
    public void SkipNEntries()
    {
        var dancers = new List<Dancer>
            {DancerGenerator.CreateDancer("a"), DancerGenerator.CreateDancer("b"), DancerGenerator.CreateDancer("c")};
        dancers.ForEach(d => _fixture._context.Dancers.Add(d));
        _fixture._context.SaveChanges();

        var result = _dancerRepository.GetDancers(2, 2);
        
        Assert.Single(result);
        Assert.Equal("c", result.First().DdrName);
    }

    #endregion
}