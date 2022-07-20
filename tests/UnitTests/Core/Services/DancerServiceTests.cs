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
        
        #region GetDancerById Tests

        [Fact(DisplayName = "When GetDancerById, if dancer exists in repository, then return Success with Dancer")]
        public async Task GetDancerByIdAsync_DancerFound()
        {
            var dancer = new Dancer
            {
                Id = Guid.NewGuid()
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerById(It.Is<Guid>(value => value.Equals(dancer.Id)))
            ).Returns(dancer);

            var result = _dancerService.GetDancerById(dancer.Id);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(dancer.Id, result.Value.Id);
        }

        [Fact(DisplayName = "When GetDancerById, if dancer not found in source, then return NotFound result")]
        public async Task GetDancerByIdAsync_DancerNotFound()
        {
            var id = Guid.NewGuid();

            _dancerRepository2.Setup(r =>
                r.GetDancerById(It.IsAny<Guid>())
            ).Returns(null as Dancer);

            var result = _dancerService.GetDancerById(id);

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

        #region UpdateDancerAsync

        [Fact(DisplayName = "When dancer is returned from repository, result is updated")]
        public async Task WhenDancerFound_ThenUpdateDancer()
        {
            var initialDancer = new Dancer
            {
                Id = Guid.NewGuid(),
                AuthenticationId = Guid.NewGuid().ToString(),
                DdrCode = "123",
                DdrName = "Abc",
                PrimaryMachineLocation = "Abc123",
                ProfilePictureTimestamp = DateTime.UtcNow,
                State = "Zyx"
            };
            var requestModel = new UpdateDancerRequestModel
            {
                AuthId = initialDancer.AuthenticationId,
                DdrCode = "456",
                DdrName = "Def",
                PrimaryMachineLocation = "Def456",
                State = "Wvu"
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value.Equals(initialDancer.AuthenticationId)))
            ).Returns(initialDancer);

            var result = await _dancerService.UpdateDancerAsync(requestModel, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(initialDancer.Id, result.Value.Id);

            _dancerRepository2.Verify(r =>
                r.UpdateDancer(It.Is<Dancer>(value =>
                    value.State == requestModel.State &&
                    value.AuthenticationId == requestModel.AuthId &&
                    value.DdrCode == requestModel.DdrCode &&
                    value.DdrName == requestModel.DdrName &&
                    value.PrimaryMachineLocation == requestModel.PrimaryMachineLocation &&
                    value.Id == initialDancer.Id
                ), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact(DisplayName = "When dancer is not found in repository, return not found result")]
        public async Task UpdateDancerAsync_DancerNotFound_NotFoundResult()
        {
            var requestModel = new UpdateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString(),
                DdrCode = "456",
                DdrName = "Def",
                PrimaryMachineLocation = "Def456",
                State = "Wvu"
            };
            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.IsAny<string>())
            ).Returns(null as Dancer);
        
            var result = await _dancerService.UpdateDancerAsync(requestModel, CancellationToken.None);
            
            Assert.Equal(ResultStatus.NotFound, result.Status);
            _dancerRepository2.Verify(r =>
                r.UpdateDancer(It.IsAny<Dancer>(), It.IsAny<CancellationToken>()), Times.Never
            );
        }

        #endregion

        #region CreateDancerAsync

        [Fact(DisplayName = "When dancer does not exist in repository, then Create is called")]
        public async Task CreateDancerAsync_DancerDoesNotExist_CreateDancer()
        {
            var requestModel = new CreateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString()
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value.Equals(requestModel.AuthId)))
            ).Returns(null as Dancer);

            var result = await _dancerService.CreateDancerAsync(requestModel, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.True(
                result.Value.AuthenticationId == requestModel.AuthId
            );
            
            _dancerRepository2.Verify(r =>
                r.CreateDancer(It.Is<Dancer>(value =>
                    value.AuthenticationId == requestModel.AuthId
                    ), It.IsAny<CancellationToken>()
                ), Times.Once
            );
        }

        [Fact(DisplayName = "When dancer already exists, then return error")]
        public async Task CreateDancerAsync_DancerExists_RaiseError()
        {
            var requestModel = new CreateDancerRequestModel
            {
                AuthId = Guid.NewGuid().ToString()
            };
            var repositoryResponse = new Dancer
            {
                Id = Guid.NewGuid(),
                AuthenticationId = requestModel.AuthId
            };

            _dancerRepository2.Setup(r =>
                r.GetDancerByAuthId(It.Is<string>(value => value.Equals(requestModel.AuthId)))
            ).Returns(repositoryResponse);
            
            var result = await _dancerService.CreateDancerAsync(requestModel, CancellationToken.None);
            Assert.Equal(ResultStatus.Error,result.Status);
            _dancerRepository2.Verify(r =>
                r.UpdateDancer(It.IsAny<Dancer>(), It.IsAny<CancellationToken>()), Times.Never
            );
        }

        #endregion

        #region GetDancerBadges

        [Fact(DisplayName = "When GetDancerBadges, if dancer has badges in repository, return success")]
        public void GetDancerBadges_DancerHasBadges_Success()
        {
            var id = Guid.NewGuid();
            var badges = new List<GetDancerBadgesResponseModel>() {new GetDancerBadgesResponseModel {Id = Guid.NewGuid()}};

            _dancerRepository2.Setup(r =>
                r.GetBadgesForDancer(It.Is<Guid>(value => value.Equals(id)))
            ).Returns(badges);

            var result = _dancerService.GetDancerBadges(id);
            
            Assert.Equal(ResultStatus.Ok, result.Status);
            Assert.Equal(badges, result.Value);
        }

        [Fact(DisplayName = "When GetDancerBadges, if dancer has no badges in repository, return empty success result")]
        public void GetDancerBadges_DancerHasNoBadges_NotFound()
        {
            var id = Guid.NewGuid();

            _dancerRepository2.Setup(r =>
                r.GetBadgesForDancer(It.Is<Guid>(value => value.Equals(id)))
            ).Returns(new List<GetDancerBadgesResponseModel>());

            var result = _dancerService.GetDancerBadges(id);
            
            Assert.Equal(ResultStatus.Ok, result.Status);
            Assert.Empty(result.Value);
        }

        #endregion

        #region AddBadgeToDancer Tests

        [Fact(DisplayName = "When AddBadgeToDancer, delegates request to repository")]
        public async Task AddBadgeToDancer_Created()
        {
            var dancerGuid = Guid.NewGuid();
            var badgeGuid = Guid.NewGuid();
            _dancerRepository2.Setup(r =>
                r.AddBadgeToDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);

            var result = await _dancerService.AddBadgeToDancer(dancerGuid, badgeGuid, CancellationToken.None);
            Assert.True(result);
            _dancerRepository2.Verify(r =>
                r.AddBadgeToDancer(
                    It.Is<Guid>(value => value.Equals(dancerGuid)),
                    It.Is<Guid>(value => value.Equals(badgeGuid)),
                    It.IsAny<CancellationToken>()
                    ), Times.Once
                );
        }

        #endregion
        
        
        #region RemoveBadgeFromDancer Tests

        [Fact(DisplayName = "When RemoveBadgeFromDancer, delegates request to repository")]
        public async Task RemoveBadgeFromDancer_Created()
        {
            var dancerGuid = Guid.NewGuid();
            var badgeGuid = Guid.NewGuid();
            _dancerRepository2.Setup(r =>
                r.RemoveBadgeFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);

            var result = await _dancerService.RemoveBadgeFromDancer(dancerGuid, badgeGuid, CancellationToken.None);
            Assert.True(result);
            _dancerRepository2.Verify(r =>
                    r.RemoveBadgeFromDancer(
                        It.Is<Guid>(value => value.Equals(dancerGuid)),
                        It.Is<Guid>(value => value.Equals(badgeGuid)),
                        It.IsAny<CancellationToken>()
                    ), Times.Once
            );
        }
        
        #endregion
    }
}