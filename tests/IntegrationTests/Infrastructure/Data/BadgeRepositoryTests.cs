using System;
using System.Linq;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Badge repository collection")]
public class BadgeRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly BadgeRepository _badgeRepository;

    public BadgeRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _badgeRepository = new BadgeRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    private void AddBadgeToTable(Badge b)
    {
        _fixture._context.Badges.Add(b);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
    }

    #region GetBadgeById

    [Fact(DisplayName = "Returns badge when exists in table")]
    public void GetBadgeById_ReturnWhenFound()
    {
        var badge = BadgeGenerator.CreateBadge();
        AddBadgeToTable(badge);

        var result = _badgeRepository.GetBadgeById(badge.Id);

        Assert.NotNull(result);
        Assert.Equal(badge.Id, result.Id);
    }

    [Fact(DisplayName = "Returns null when does not exist in table")]
    public void GetBadgeById_NullWhenNotFound()
    {
        var badge = BadgeGenerator.CreateBadge();
        AddBadgeToTable(badge);

        var result = _badgeRepository.GetBadgeById(Guid.NewGuid());

        Assert.Null(result);
    }

    #endregion

    #region GetBadges

    [Fact(DisplayName = "When badges exists, no event, return badge details")]
    public void GetBadges_BadgesExist_NoEvent()
    {
        var badge = BadgeGenerator.CreateBadge();
        AddBadgeToTable(badge);

        var badges = _badgeRepository.GetBadges(0, 10).ToList();

        Assert.Single(badges);
        Assert.Null(badges.First().EventName);
    }

    [Fact(DisplayName = "When badge exists, has event, return badge with event name")]
    public void GetBadges_BadgesExists_EventExists()
    {
        var e = EventGenerator.CreateEvent();
        e.Name = "Event Name";
        _fixture._context.Events.Add(e);
        var badge = BadgeGenerator.CreateBadge();
        badge.EventId = e.Id;
        badge.Event = e;
        AddBadgeToTable(badge);

        var badges = _badgeRepository.GetBadges(0, 10).ToList();

        Assert.Single(badges);
        Assert.Equal(e.Name, badges.First().EventName);
    }

    #endregion
}