using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Badge;
using Application.Core.Services;
using Moq;
using Xunit;

namespace UnitTests.Core.Services;

public class BadgeServiceTests
{
    private readonly Mock<IBadgeRepository> _badgeRepository2;
    private readonly Mock<IAsyncRepository<Badge>> _badgeRepository;
    private readonly IBadgeService _badgeService;

    public BadgeServiceTests()
    {
        _badgeRepository2 = new Mock<IBadgeRepository>();
        _badgeRepository = new Mock<IAsyncRepository<Badge>>();
        _badgeService = new BadgeService(_badgeRepository2.Object, _badgeRepository.Object);
    }

    #region GetBadges

    [Fact(DisplayName = "When GetBadges, repository is called with skip and take")]
    public void When_GetBadgesAsync_Then_PagedRequestToRepository()
    {
        _badgeRepository2.Setup(r =>
            r.GetBadges(
                It.IsAny<int>(), It.IsAny<int>())
            ).Returns(new List<GetBadgesResponseModel>(){new GetBadgesResponseModel()});

        var result = _badgeService.GetBadges(2, 10);
        
        Assert.Single(result);
        
        _badgeRepository2.Verify(r =>
            r.GetBadges(It.Is<int>(value => value == 20), It.Is<int>(value => value == 10)), Times.Once);
    }

    #endregion

    #region CreateBadgeAsync

    [Fact(DisplayName = "When CreateBadgeAsync, repository is called with new badge")]
    public async Task When_CreateBadgeAsync_Then_RepositoryIsCalledWithBadge()
    {
        var badge = new Badge
        {
            Id = Guid.NewGuid()
        };

        _badgeRepository.Setup(r => 
            r.AddAsync(It.IsAny<Badge>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(badge);

        var result = await _badgeService.CreateBadgeAsync(badge, CancellationToken.None);
        
        _badgeRepository.Verify(r =>
            r.AddAsync(badge, It.IsAny<CancellationToken>()));
        
        Assert.True(result.IsSuccess);
        Assert.Equal(badge, result.Value);
    }

    #endregion
}