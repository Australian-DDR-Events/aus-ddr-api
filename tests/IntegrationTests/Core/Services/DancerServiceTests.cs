using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Models.Dancer;
using Application.Core.Services;
using Ardalis.Result;
using AusDdrApi.Services.FileStorage;
using Infrastructure.Data;
using Xunit;

namespace IntegrationTests.Core.Services
{
    [Collection("Postgres database collection")]
    public class DancerServiceTests
    {
        private readonly PostgresDatabaseFixture _fixture;
        private readonly IAsyncRepository<Badge> _badgeRepository;
        private readonly IAsyncRepository<Dancer> _dancerRepository;
        private readonly IFileStorage _fileStorage;
        private readonly DancerService _dancerService;

        public DancerServiceTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _badgeRepository = new GenericEfRepository<Badge>(_fixture._context);
            _dancerRepository = new GenericEfRepository<Dancer>(_fixture._context);
            _fileStorage = new LocalFileStorage("");
            _dancerService = new DancerService(_dancerRepository, _badgeRepository, _fileStorage);
            
            Setup.DropAllRows(_fixture._context);
        }

        #region GetDancerByIdAsync Tests
        
        [Fact(DisplayName = "If data source contains dancer id, then return the dancer")]
        public async Task GetDancerByIdAsync_ReturnDancerIfFound()
        {
            var dancerGuid = Guid.NewGuid();
            var dancer = new Dancer()
            {
                Id = dancerGuid
            };
            _fixture._context.Dancers.Add(dancer);
            await _fixture._context.SaveChangesAsync();

            var dancerFromDatabase = await _dancerService.GetByIdAsync(dancerGuid, CancellationToken.None);
            
            Assert.True(dancerFromDatabase.IsSuccess);
            Assert.Equal(dancerFromDatabase.Value.Id, dancer.Id);
        }

        [Fact(DisplayName = "If data source does not contain dancer id, then return not found result")]
        public async Task GetDancerByIdAsync_ReturnNotFoundIfNotExist()
        {
            var invalidGuid = Guid.NewGuid();
            var dancer = new Dancer()
            {
                Id = Guid.NewGuid()
            };
            await _fixture._context.Dancers.AddAsync(dancer);
            await _fixture._context.SaveChangesAsync();

            var dancerFromDatabase = await _dancerService.GetByIdAsync(invalidGuid, CancellationToken.None);
            
            Assert.Equal(ResultStatus.NotFound, dancerFromDatabase.Status);
        }
        
        #endregion
        
        #region GetDancersAsync tests

        [Fact(DisplayName = "If data source contains at least the number of dancers, return list of number of dancers")]
        public async Task GetDancersAsync_ReturnNumberOfDancersEqualToTake()
        {
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
            await _fixture._context.Dancers.AddRangeAsync(dancers);
            await _fixture._context.SaveChangesAsync();

            var dancersFromDatabase = await _dancerService.GetDancersAsync(0, 2, CancellationToken.None);
            
            Assert.True(dancersFromDatabase.IsSuccess);
            Assert.Equal(dancers.OrderBy(d => d.Id).Take(2), dancersFromDatabase.Value);
        }
        
        [Fact(DisplayName = "If data requested is out of range, return empty list")]
        public async Task GetDancersAsync_ReturnEmptyListWhenOutOfRange()
        {
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
            await _fixture._context.Dancers.AddRangeAsync(dancers);
            await _fixture._context.SaveChangesAsync();

            var dancersFromDatabase = await _dancerService.GetDancersAsync(3, 2, CancellationToken.None);
            
            Assert.True(dancersFromDatabase.IsSuccess);
            Assert.Empty(dancersFromDatabase.Value);
        }
        
        #endregion

        #region UpdateDancerAsync Tests

        [Fact(DisplayName = "If database does not contain auth id, create new entry")]
        public async Task InsertDancerByUpdateWhenDancerDoesNotExist()
        {
            var request = new UpdateDancerRequestModel
            {
                AuthId = "my-auth-id",
                DdrCode = "123",
                DdrName = "test-acc",
                PrimaryMachineLocation = "Crown",
                State = "vic"
            };

            var newDancer = await _dancerService.UpdateDancerAsync(request, CancellationToken.None);
            var expectedDancer = await _fixture._context.Dancers.FindAsync(newDancer.Value.Id);

            Assert.NotNull(expectedDancer);
        }

        [Fact(DisplayName = "If id matches auth id, update existing entry")]
        public async Task UpdateDancerByAuthId()
        {
            var dancer = new Dancer()
            {
                AuthenticationId = "sample-123",
                DdrName = "Bob"
            };
            await _fixture._context.Dancers.AddAsync(dancer);

            var request = new UpdateDancerRequestModel()
            {
                AuthId = "sample-123",
                DdrName = "Bill",
                State = "vic"
            };
            
            var updatedDancer = await _dancerService.UpdateDancerAsync(request, CancellationToken.None);
            
            Assert.Equal("sample-123", updatedDancer.Value.AuthenticationId);
            Assert.Equal("Bill", updatedDancer.Value.DdrName);
            Assert.Equal("vic", updatedDancer.Value.State);
        }

        [Fact(DisplayName = "If id matches legacy id, update existing entry")]
        public async Task UpdateDancerByLegacyId()
        {
            var dancer = new Dancer()
            {
                AuthenticationId = "sample-123",
                DdrName = "Bob"
            };
            await _fixture._context.Dancers.AddAsync(dancer);

            var request = new UpdateDancerRequestModel()
            {
                AuthId = "new-id",
                DdrName = "Bob"
            };
            
            var updatedDancer = await _dancerService.UpdateDancerAsync(request, CancellationToken.None);
            
            Assert.Equal("new-id", updatedDancer.Value.AuthenticationId);
        }

        #endregion
    }
}