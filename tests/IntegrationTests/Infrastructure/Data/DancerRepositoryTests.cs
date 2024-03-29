using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Dancer repository collection")]
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

    private void AddRewardQuality(RewardQuality rewardQuality)
    {
        _fixture._context.RewardQualities.Add(rewardQuality);
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

    #region AddBadgeToDancer

    [Fact(DisplayName = "When dancer exists, badge not assigned, then add badge to dancer")]
    public async Task AddBadgeToDancer_DancerExists_BadgeNotAssigned_AssignBadge()
    {
        var badge = BadgeGenerator.CreateBadge();
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.Add(badge);
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        await _dancerRepository.AddBadgeToDancer(dancer.Id, badge.Id, CancellationToken.None);
        _fixture._context.ChangeTracker.Clear();

        var dancerBadgeResult = _dancerRepository.GetBadgesForDancer(dancer.Id);
        var badges = dancerBadgeResult.ToList();
        Assert.Single(badges);
        Assert.Equal(badges.First().Id, badge.Id);
    }

    [Fact(DisplayName = "When dancer exists, badge does not exist, then do not change dancer badges")]
    public async Task AddBadgeToDancer_DancerExists_BadgeNotExists_NoChangeToDancer()
    {
        var badge = BadgeGenerator.CreateBadge();
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.Add(badge);
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        await _dancerRepository.AddBadgeToDancer(dancer.Id, Guid.NewGuid(), CancellationToken.None);
        _fixture._context.ChangeTracker.Clear();

        var dancerBadgeResult = _dancerRepository.GetBadgesForDancer(dancer.Id);
        var badges = dancerBadgeResult.ToList();
        Assert.Empty(badges);
    }

    [Fact(DisplayName = "When dancer exists, add multiple badges, dancer has multiple badges")]
    public async Task AddBadgeToDancer_DancerExists_AddMultipleBadges_AllBadgesAssigned()
    {
        var badges = new List<Badge>()
            {BadgeGenerator.CreateBadge(), BadgeGenerator.CreateBadge(), BadgeGenerator.CreateBadge()};
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.AddRange(badges);
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        foreach (var iteration in badges.Select((badge, i) => new { i, badge }))
        {
            await _dancerRepository.AddBadgeToDancer(dancer.Id, iteration.badge.Id, CancellationToken.None);
            _fixture._context.ChangeTracker.Clear();
            
            var dancerBadgeResult = _dancerRepository.GetBadgesForDancer(dancer.Id).ToList();
            Assert.Equal(iteration.i + 1, dancerBadgeResult.Count);
            Assert.Contains(dancerBadgeResult, b => b.Id.Equals(iteration.badge.Id));
        }
    }

    [Fact(DisplayName = "When dancer not found, do not throw exception")]
    public async Task AddBadgeToDancer_DancerNotFound_NoException()
    {
        var badge = BadgeGenerator.CreateBadge();
        _fixture._context.Badges.Add(badge);
        await _fixture._context.SaveChangesAsync();

        var exception = await Record.ExceptionAsync(async () =>
            await _dancerRepository.AddBadgeToDancer(Guid.NewGuid(), badge.Id, CancellationToken.None)
        );

        Assert.Null(exception);
    }

    [Fact(DisplayName = "When dancer exists, badge not found, do not throw exception")]
    public async Task AddBadgeToDancer_DancerFound_BadgeNotFound_NoException()
    {
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        var exception = await Record.ExceptionAsync(async () =>
            await _dancerRepository.AddBadgeToDancer(dancer.Id, Guid.NewGuid(), CancellationToken.None)
        );

        Assert.Null(exception);
    }

    #endregion

    #region RemoveBadgeFromDancer

    [Fact(DisplayName = "When dancer exists, has badge, remove badge from dancer")]
    public async Task RemoveBadgeFromDancer_BadgeAssigned_BadgeIsRemoved()
    {
        var badge = BadgeGenerator.CreateBadge();
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.Add(badge);
        dancer.Badges = new List<Badge>() {badge};
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();

        var repositoryResult = await _dancerRepository.RemoveBadgeFromDancer(dancer.Id, badge.Id, CancellationToken.None);
        _fixture._context.ChangeTracker.Clear();

        var databaseResult = _dancerRepository.GetBadgesForDancer(dancer.Id);
        
        Assert.True(repositoryResult);
        Assert.Empty(databaseResult);
    }

    [Fact(DisplayName = "When dancer does not exist, result is false")]
    public async Task RemoveBadgeFromDancer_DancerNotExist_FalseResult()
    {
        var badge = BadgeGenerator.CreateBadge();
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.Add(badge);
        dancer.Badges = new List<Badge>() {badge};
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();
        
        var repositoryResult = await _dancerRepository.RemoveBadgeFromDancer(Guid.NewGuid(), badge.Id, CancellationToken.None);
        
        Assert.False(repositoryResult);
    }

    [Fact(DisplayName = "When dancer exist, badge not found on dancer, result is false")]
    public async Task RemoveBadgeFromDancer_DancerDoesNotHaveBadge_FalseResult()
    {
        var badge = BadgeGenerator.CreateBadge();
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Badges.Add(badge);
        _fixture._context.Dancers.Add(dancer);
        await _fixture._context.SaveChangesAsync();
        
        var repositoryResult = await _dancerRepository.RemoveBadgeFromDancer(Guid.NewGuid(), badge.Id, CancellationToken.None);
        
        Assert.False(repositoryResult);
    }

    #endregion

    #region RemoveRewardFromDancer

    [Fact(DisplayName = "When dancer exists, has reward, remove reward from dancer")]
    public async Task RemoveRewardFromDancer_RewardAssigned_RewardIsRemoved()
    {
        var reward = RewardGenerator.CreateReward();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Rewards.Add(reward);
        _fixture._context.RewardQualities.Add(rewardQuality);
        dancer.RewardQualities = new List<RewardQuality>() {rewardQuality};
        AddDancerToTable(dancer);

        var repositoryResult = await _dancerRepository.RemoveRewardFromDancer(rewardQuality.Id, dancer.Id, CancellationToken.None);
        _fixture._context.ChangeTracker.Clear();

        var databaseResult = _fixture._context.Dancers.Include(d => d.RewardQualities)
            .First(d => d.Id.Equals(dancer.Id));
        
        Assert.True(repositoryResult);
        Assert.Empty(databaseResult.RewardQualities);
    }
    //
    [Fact(DisplayName = "When dancer does not exist, result is false")]
    public async Task RemoveRewardFromDancer_DancerNotExist_FalseResult()
    {
        var reward = RewardGenerator.CreateReward();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Rewards.Add(reward);
        _fixture._context.RewardQualities.Add(rewardQuality);
        dancer.RewardQualities = new List<RewardQuality>() {rewardQuality};
        AddDancerToTable(dancer);
        
        var repositoryResult = await _dancerRepository.RemoveRewardFromDancer(Guid.NewGuid(), rewardQuality.Id, CancellationToken.None);
        
        Assert.False(repositoryResult);
    }
    //
    [Fact(DisplayName = "When dancer exist, reward not found on dancer, result is false")]
    public async Task RemoveRewardFromDancer_DancerDoesNotHaveReward_FalseResult()
    {
        var reward = RewardGenerator.CreateReward();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Rewards.Add(reward);
        _fixture._context.RewardQualities.Add(rewardQuality);
        AddDancerToTable(dancer);
        
        var repositoryResult = await _dancerRepository.RemoveRewardFromDancer(Guid.NewGuid(), dancer.Id, CancellationToken.None);
        
        Assert.False(repositoryResult);
    }

    #endregion
}
