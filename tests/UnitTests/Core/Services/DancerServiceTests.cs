using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Application.Core.Services;
using Application.Core.Specifications;
using Ardalis.Result;
using Ardalis.Specification;
using Moq;
using Xunit;

namespace UnitTests.Core.Services
{
    public class DancerServiceTests
    {
        private readonly Mock<IAsyncRepository<Dancer>> _repository;
        private readonly IDancerService _dancerService;
        
        public DancerServiceTests()
        {
            _repository = new Mock<IAsyncRepository<Dancer>>();
            _dancerService = new DancerService(_repository.Object);
        }
        
        #region GetDancerByIdAsync Tests

        [Fact(DisplayName = "When GetDancerById, if dancer exists in source, then return Success with Dancer")]
        public async Task GetDancerByIdAsync_DancerFound()
        {
            var id = new Guid();
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

            _repository.Setup(r =>
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
            var id = new Guid();
            var expectedSpec = new ByIdSpec<Dancer>(id);

            _repository.Setup(r =>
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

        [Fact(DisplayName = "If data source contains at least the number of dancers, return list of number of dancers")]
        public async Task GetDancersAsync_ReturnNumberOfDancersEqualToTake()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var dancers = new List<Dancer>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                }
            };
            Task.WaitAll(dancers.Select(d => repository.AddAsync(d)).ToArray());
            await repository.SaveChangesAsync();

            var dancersFromDatabase = await service.GetDancersAsync(0, 2, CancellationToken.None);
            
            Assert.True(dancersFromDatabase.IsSuccess);
            Assert.Equal(dancers.OrderBy(d => d.Id).Take(2), dancersFromDatabase.Value);
        }
        
        [Fact(DisplayName = "If data requested is out of range, return empty list")]
        public async Task GetDancersAsync_ReturnEmptyListWhenOutOfRange()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var dancers = new List<Dancer>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AuthenticationId = Guid.NewGuid().ToString()
                }
            };
            
            Task.WaitAll(dancers.Select(d => repository.AddAsync(d)).ToArray());
            await repository.SaveChangesAsync();

            var dancersFromDatabase = await service.GetDancersAsync(3, 2, CancellationToken.None);
            
            Assert.True(dancersFromDatabase.IsSuccess);
            Assert.Empty(dancersFromDatabase.Value);
            
        }
        
        #endregion

        #region UpdateDancerAsync Tests

        [Fact(DisplayName = "If database does not contain auth id, create new entry")]
        public async Task InsertDancerByUpdateWhenDancerDoesNotExist()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var request = new UpdateDancerRequestModel
            {
                AuthId = "my-auth-id",
                DdrCode = "123",
                DdrName = "test-acc",
                PrimaryMachineLocation = "Crown",
                State = "vic"
            };

            var newDancer = await service.UpdateDancerAsync(request, CancellationToken.None);
            var expectedDancer = await repository.GetByIdAsync(newDancer.Value.Id);

            Assert.NotNull(expectedDancer);
        }

        [Fact(DisplayName = "If id matches auth id, update existing entry")]
        public async Task UpdateDancerByAuthId()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var dancer = new Dancer()
            {
                AuthenticationId = "sample-123",
                DdrName = "Bob"
            };
            await repository.AddAsync(dancer);
            await repository.SaveChangesAsync();

            var request = new UpdateDancerRequestModel()
            {
                AuthId = "sample-123",
                DdrName = "Bill",
                State = "vic"
            };
            
            var updatedDancer = await service.UpdateDancerAsync(request, CancellationToken.None);
            
            Assert.Equal("sample-123", updatedDancer.Value.AuthenticationId);
            Assert.Equal("Bill", updatedDancer.Value.DdrName);
            Assert.Equal("vic", updatedDancer.Value.State);
        }

        [Fact(DisplayName = "If id matches legacy id, update existing entry")]
        public async Task UpdateDancerByLegacyId()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var dancer = new Dancer()
            {
                AuthenticationId = "sample-123",
                DdrName = "Bob"
            };
            await repository.AddAsync(dancer);
            await repository.SaveChangesAsync();

            var request = new UpdateDancerRequestModel()
            {
                AuthId = "new-id",
                DdrName = "Bob"
            };
            
            var updatedDancer = await service.UpdateDancerAsync(request, CancellationToken.None);
            
            Assert.Equal("new-id", updatedDancer.Value.AuthenticationId);
        }

        #endregion
    }
}