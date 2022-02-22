using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Services;
using Application.Core.Specifications;
using Moq;
using Xunit;

namespace UnitTests.Core.Services;

public class BadgeServiceTests
{
    private readonly Mock<IAsyncRepository<Badge>> _badgeRepository;
    private readonly IBadgeService _badgeService;

    public BadgeServiceTests()
    {
        _badgeRepository = new Mock<IAsyncRepository<Badge>>();
        _badgeService = new BadgeService(_badgeRepository.Object);
    }

    #region GetBadgesAsync

    [Fact(DisplayName = "When GetBadgesAsync, repository is called with paging spec")]
    public async Task When_GetBadgesAsync_Then_PagedRequestToRepository()
    {
        _badgeRepository.Setup(r =>
            r.ListAsync(
                It.IsAny<PageableSpec<Badge>>(), 
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<Badge>());

        var result = await _badgeService.GetBadgesAsync(2, 10, CancellationToken.None);
        
        // TODO: validate spec with paging details
        
        Assert.True(result.IsSuccess);
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