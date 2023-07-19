using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Dancer service collection")]
public class CommonRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly CommonRepository<Dancer> _commonRepository;

    public CommonRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _commonRepository = new CommonRepository<Dancer>(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    [Fact(DisplayName = "Get N entities")]
    public void LimitToNEntries()
    {
        var dancers = new List<Dancer>
            {DancerGenerator.CreateDancer(), DancerGenerator.CreateDancer(), DancerGenerator.CreateDancer()};
        dancers.ForEach(d => _fixture._context.Dancers.Add(d));
        _fixture._context.SaveChanges();

        var result = _commonRepository.GetN(0, 2, dancer => dancer, false, dancer => dancer.Id);
        
        Assert.Equal(2, result.Count());
    }
}