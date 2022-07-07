using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Application.Core.Services;
using Application.Core.Specifications;
using Application.Core.Specifications.DancerSpecs;
using Ardalis.Result;
using Moq;
using Xunit;

namespace UnitTests.Core.Services
{
    public class DancerServiceTests
    {
        private readonly Mock<IAsyncRepository<Dancer>> _dancerRepository;
        private readonly Mock<IAsyncRepository<Badge>> _badgeRepository;
        private readonly Mock<IDancerRepository> _dancerRepository2;
        private readonly Mock<IFileStorage> _fileStorage;
        private readonly IDancerService _dancerService;
        
        public DancerServiceTests()
        {
            _dancerRepository = new Mock<IAsyncRepository<Dancer>>();
            _badgeRepository = new Mock<IAsyncRepository<Badge>>();
            _dancerRepository2 = new Mock<IDancerRepository>();
            _fileStorage = new Mock<IFileStorage>();
            _dancerService = new DancerService(_dancerRepository.Object, _badgeRepository.Object, _dancerRepository2.Object, _fileStorage.Object);
        }
        
        #region GetDancerByIdAsync Tests

        [Fact(DisplayName = "When GetDancerById, if dancer exists in source, then return Success with Dancer")]
        public async Task GetDancerByIdAsync_DancerFound()
        {
            var id = Guid.NewGuid();
            var expectedSpec = new ByIdSpec<Dancer>(id);

            var repositoryDancer = new Dancer
            {
                Id = id,
                DdrName = "Name",
                DdrCode = "Code",
                AuthenticationId = "auth",
                PrimaryMachineLocation = "Home",
                State = "vic"
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.Is<ByIdSpec<Dancer>>(s => s.ToString().Equals(expectedSpec.ToString())),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryDancer);

            var result = await _dancerService.GetByIdAsync(id, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryDancer, result.Value);
        }

        [Fact(DisplayName = "When GetDancerById, if dancer not found in source, then return NotFound result")]
        public async Task GetDancerByIdAsync_DancerNotFound()
        {
            var id = Guid.NewGuid();
            var expectedSpec = new ByIdSpec<Dancer>(id);

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.Is<ByIdSpec<Dancer>>(s => s.ToString().Equals(expectedSpec.ToString())),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dancer) null);

            var result = await _dancerService.GetByIdAsync(id, CancellationToken.None);
            
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.NotFound, result.Status);
        }

        
        #endregion
        
        #region GetDancersAsync tests

        [Fact(DisplayName = "GetDancers is called on the repository with correct skip and take")]
        public async Task WhenGetDancersAsync_GetDancersCalledWithTakeAndSkip()
        {
            var repositoryResponse = new List<Dancer>()
            {
                new()
                {
                    Id = Guid.NewGuid()
                }
            };

            _dancerRepository2.Setup(r => 
                r.GetDancers(It.IsAny<int>(), It.IsAny<int>())
            ).Returns(repositoryResponse);
        
            var result = await _dancerService.GetDancersAsync(2, 4, new CancellationToken());
        
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryResponse, result.Value);
        
            _dancerRepository2.Verify(mock => mock.GetDancers(
                    It.Is<int>(n => n == 8), 
                    It.Is<int>(n => n == 4)
                ), Times.Once()
            );
        }
        
        #endregion

        #region MigrateDancer

        [Fact(DisplayName = "When dancer found by authId, update is not called")]
        public async Task WhenMigrateDancer_FoundByAuthId_NoUpdate()
        {
            var repositoryResponse = new Dancer
            {
                Id = Guid.NewGuid()
            };

            var requestModel = new MigrateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString(),
                LegacyId = Guid.NewGuid().ToString()
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.AuthId))
            ).Returns(repositoryResponse);

            var result = await _dancerService.MigrateDancer(requestModel, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryResponse.Id, result.Value.Id);
            
            _dancerRepository2.Verify(mock => mock.UpdateDancer(It.IsAny<Dancer>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "When dancer found by legacyId, update is called with new id")]
        public async Task WhenMigrateDancer_FoundByLegacyId_Update()
        {
            var repositoryResponse = new Dancer
            {
                Id = Guid.NewGuid()
            };

            var requestModel = new MigrateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString(),
                LegacyId = Guid.NewGuid().ToString()
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.AuthId))
            ).Returns(null as Dancer);
            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.LegacyId))
            ).Returns(repositoryResponse);

            var result = await _dancerService.MigrateDancer(requestModel, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryResponse.Id, result.Value.Id);
            
            _dancerRepository2.Verify(
                mock => mock.UpdateDancer(
                    It.Is<Dancer>(d => d.AuthenticationId.Equals(requestModel.AuthId)),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact(DisplayName = "When legacyId not provided, and not found by authId, return not found")]
        public async Task WhenMigrateDancer_NotFoundByAuthId_LegacyIdNotProvided_ReturnNotFound()
        {
            var requestModel = new MigrateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString(),
                LegacyId = null
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.AuthId))
            ).Returns(null as Dancer);

            var result = await _dancerService.MigrateDancer(requestModel, CancellationToken.None);
            
            Assert.Equal(ResultStatus.NotFound, result.Status);
            
            _dancerRepository2.Verify(
                mock => mock.UpdateDancer(It.IsAny<Dancer>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact(DisplayName = "When legacyId not provided, and not found by authId, return not found")]
        public async Task WhenMigrateDancer_NotFoundByAuthId_NotFoundByLegacyId_ReturnNotFound()
        {
            var requestModel = new MigrateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString(),
                LegacyId = Guid.NewGuid().ToString()
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.AuthId))
            ).Returns(null as Dancer);
            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value == requestModel.LegacyId))
            ).Returns(null as Dancer);

            var result = await _dancerService.MigrateDancer(requestModel, CancellationToken.None);
            
            Assert.Equal(ResultStatus.NotFound, result.Status);
            
            _dancerRepository2.Verify(
                mock => mock.UpdateDancer(It.IsAny<Dancer>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        #endregion

        #region AddBadgeToDancer Tests

        [Fact(DisplayName = "When AddBadgeToDancer, if dancer and badge exists in source, then return success")]
        public async Task AddBadgeToDancer_Created()
        {
            var badgeId = Guid.NewGuid();
            var dancerId = Guid.NewGuid();

            var repositoryDancer = new Dancer
            {
                Id = dancerId,
                Badges = new List<Badge>()
            };
            var repositoryBadge = new Badge
            {
                Id = badgeId
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.IsAny<DancerBadgesSpec>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryDancer);

            _badgeRepository.Setup(r =>
                    r.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryBadge);

            var result = await _dancerService.AddBadgeToDancer(dancerId, badgeId, CancellationToken.None);

            _badgeRepository.Verify(repository => repository.GetByIdAsync(badgeId, It.IsAny<CancellationToken>()), Times.Once);
            _dancerRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
            
            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "When AddBadgeToDancer, if dancer not found, then return not found")]
        public async Task AddBadgeToDancer_NoDancerFound_NotFound()
        {
            var badgeId = Guid.NewGuid();
            var dancerId = Guid.NewGuid();

            var repositoryBadge = new Badge
            {
                Id = badgeId
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.IsAny<DancerBadgesSpec>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dancer) null);

            _badgeRepository.Setup(r =>
                    r.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryBadge);

            var result = await _dancerService.AddBadgeToDancer(dancerId, badgeId, CancellationToken.None);
            
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.NotFound, result.Status);

            _badgeRepository.Verify(repository => repository.GetByIdAsync(badgeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "When AddBadgeToDancer, if badge not found, then return not found")]
        public async Task AddBadgeToDancer_NoBadgeFound_NotFound()
        {
            var badgeId = Guid.NewGuid();
            var dancerId = Guid.NewGuid();

            var repositoryDancer = new Dancer
            {
                Id = dancerId,
                Badges = new List<Badge>()
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.IsAny<DancerBadgesSpec>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryDancer);

            _badgeRepository.Setup(r =>
                    r.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Badge) null);

            var result = await _dancerService.AddBadgeToDancer(dancerId, badgeId, CancellationToken.None);
            
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.NotFound, result.Status);

            _badgeRepository.Verify(repository => repository.GetByIdAsync(badgeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
        
        
        #region RemoveBadgeFromDancer Tests

        [Fact(DisplayName = "When RemoveBadgeFromDancer, if dancer and badge exists in source, and dancer has badge, then return success")]
        public async Task RemoveBadgeFromDancer_Success()
        {
            var badgeId = Guid.NewGuid();
            var dancerId = Guid.NewGuid();

            var repositoryBadge = new Badge
            {
                Id = badgeId
            };
            var repositoryDancer = new Dancer
            {
                Id = dancerId,
                Badges = new List<Badge>(){repositoryBadge}
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.IsAny<DancerBadgesSpec>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryDancer);

            _badgeRepository.Setup(r =>
                    r.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryBadge);

            var result = await _dancerService.RemoveBadgeFromDancer(dancerId, badgeId, CancellationToken.None);

            _badgeRepository.Verify(repository => repository.GetByIdAsync(badgeId, It.IsAny<CancellationToken>()), Times.Once);
            _dancerRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
            
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact(DisplayName = "When RemoveBadgeFromDancer, if dancer and badge exists in source, and dancer does not have badge, then return success")]
        public async Task RemoveBadgeFromDancer_Success_NoChange()
        {
            var badgeId = Guid.NewGuid();
            var dancerId = Guid.NewGuid();

            var repositoryBadge = new Badge
            {
                Id = badgeId
            };
            var repositoryDancer = new Dancer
            {
                Id = dancerId,
                Badges = new List<Badge>()
            };

            _dancerRepository.Setup(r =>
                    r.GetBySpecAsync(
                        It.IsAny<DancerBadgesSpec>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryDancer);

            _badgeRepository.Setup(r =>
                    r.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryBadge);

            var result = await _dancerService.RemoveBadgeFromDancer(dancerId, badgeId, CancellationToken.None);

            _badgeRepository.Verify(repository => repository.GetByIdAsync(badgeId, It.IsAny<CancellationToken>()), Times.Once);
            _dancerRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
            
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }
        
        #endregion
    }
}