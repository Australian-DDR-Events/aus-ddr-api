using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    private void AddDancerToTable(Dancer d)
    {
        _fixture._context.Dancers.Add(d);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
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
        dancers.ForEach(AddDancerToTable);

        var result = _dancerRepository.GetDancers(2, 2);
        
        Assert.Single(result);
        Assert.Equal("c", result.First().DdrName);
    }

    #endregion

    #region GetDancerById

    [Fact(DisplayName = "Returns dancer when exists in table")]
    public void GetDancerById_ReturnWhenFound()
    {
        var dancer = DancerGenerator.CreateDancer();
        AddDancerToTable(dancer);

        var result = _dancerRepository.GetDancerById(dancer.Id);

        Assert.NotNull(result);
        Assert.Equal(dancer.Id, result.Id);
    }

    [Fact(DisplayName = "Returns null when does not exist in table")]
    public void GetDancerById_NullWhenNotFound()
    {
        var dancer = DancerGenerator.CreateDancer();
        AddDancerToTable(dancer);

        var result = _dancerRepository.GetDancerById(Guid.NewGuid());

        Assert.Null(result);
    }

    #endregion

    #region GetDancerByAuthId

    [Fact(DisplayName = "Returns dancer when exists in table")]
    public void GetDancerByAuthId_ReturnWhenFound()
    {
        var dancer = DancerGenerator.CreateDancer();
        AddDancerToTable(dancer);

        var result = _dancerRepository.GetDancerByAuthId(dancer.AuthenticationId);

        Assert.NotNull(result);
        Assert.Equal(dancer.Id, result.Id);
        Assert.Equal(dancer.AuthenticationId, result.AuthenticationId);
    }

    [Fact(DisplayName = "Returns null when does not exist in table")]
    public void GetDancerByAuthId_NullWhenNotFound()
    {
        var dancer = DancerGenerator.CreateDancer();
        AddDancerToTable(dancer);

        var result = _dancerRepository.GetDancerByAuthId(Guid.NewGuid().ToString());

        Assert.Null(result);
    }

    #endregion

    #region CreateDancer

    [Fact(DisplayName = "Creates a new dancer when dancer does not exist in table")]
    public async Task CreateDancer_CreateNewRecord()
    {
        var dancer = DancerGenerator.CreateDancer("InitialName");
        await _dancerRepository.CreateDancer(dancer, CancellationToken.None);

        var result = _dancerRepository.GetDancerByAuthId(dancer.AuthenticationId);
        
        Assert.NotNull(result);
        Assert.Equal(dancer.DdrName, result.DdrName);
    }

    [Fact(DisplayName = "Does not insert into table when dancer already exists")]
    public async Task CreateDancer_DancerExists_NoInsert()
    {
        var dancer = DancerGenerator.CreateDancer("InitialName");
        AddDancerToTable(dancer);

        dancer.DdrName = "NewName";
        await _dancerRepository.CreateDancer(dancer, CancellationToken.None);

        var result = _dancerRepository.GetDancerByAuthId(dancer.AuthenticationId);
        
        Assert.NotNull(result);
        Assert.Equal("InitialName", result.DdrName);
    }

    #endregion

    #region UpdateDancer

    [Fact(DisplayName = "Updates existing record when dancer exists in the table")]
    public async Task UpdateDancer_UpdateRecord()
    {
        var dancer = DancerGenerator.CreateDancer("InitialName");
        AddDancerToTable(dancer);

        dancer.DdrName = "NewName";
        await _dancerRepository.UpdateDancer(dancer, CancellationToken.None);

        var result = _dancerRepository.GetDancerById(dancer.Id);
        
        Assert.NotNull(result);
        Assert.Equal(dancer.DdrName, result.DdrName);
    }

    [Fact(DisplayName = "No change when dancer does not exist in the table")]
    public async Task UpdateDancer_NoChangeToTable()
    {
        var dancer = DancerGenerator.CreateDancer("InitialName");
        await _dancerRepository.UpdateDancer(dancer, CancellationToken.None);

        var result = _dancerRepository.GetDancerById(dancer.Id);
        
        Assert.Null(result);
    }

    #endregion

    #region GetBadgesForDancer

    [Fact(DisplayName = "When dancer has badges, return badges")]
    public async Task GetBadgesForDancer_AssignedBadges_ListOfBadges()
    {
        var badges = new List<Badge>() {BadgeGenerator.CreateBadge(), BadgeGenerator.CreateBadge()};
        var dancer = DancerGenerator.CreateDancer();
        dancer.Badges = badges;
        badges.ForEach(b => _fixture._context.Badges.Add(b));
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        var result = _dancerRepository.GetBadgesForDancer(dancer.Id);
        
        Assert.Equal(2, result.Count());
    }

    [Fact(DisplayName = "When dancer has no badges, return empty list")]
    public async Task GetBadgesForDancer_NoAssignedBadges_EmptyResult()
    {
        var badges = new List<Badge>() {BadgeGenerator.CreateBadge(), BadgeGenerator.CreateBadge()};
        var dancer = DancerGenerator.CreateDancer();
        badges.ForEach(b => _fixture._context.Badges.Add(b));
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        var result = _dancerRepository.GetBadgesForDancer(dancer.Id);
        
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When dancer not found, return empty list")]
    public async Task GetBadgesForDancer_NoDancerExists_EmptyResult()
    {
        var badges = new List<Badge>() {BadgeGenerator.CreateBadge(), BadgeGenerator.CreateBadge()};
        badges.ForEach(b => _fixture._context.Badges.Add(b));
        await _fixture._context.SaveChangesAsync();

        var result = _dancerRepository.GetBadgesForDancer(Guid.NewGuid());
        
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When dancer has badge, badge has associated event, return badges with event name")]
    public async Task GetBadgesForDancer_DancerHasBadge_BadgeHasName()
    {
        var e = EventGenerator.CreateEvent();
        e.Name = "EventName";
        var badge = BadgeGenerator.CreateBadge();
        badge.Event = e;
        var dancer = DancerGenerator.CreateDancer();
        dancer.Badges = new List<Badge>(){badge};
        _fixture._context.Events.Add(e);
        _fixture._context.Badges.Add(badge);
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        var result = _dancerRepository.GetBadgesForDancer(dancer.Id);

        var badges = result.ToList();
        Assert.Single(badges);
        Assert.Equal(e.Name, badges.ToList()[0].EventName);
    }

    #endregion
}
