using System;
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
}