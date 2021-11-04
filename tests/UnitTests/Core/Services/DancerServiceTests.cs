using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Dancer;
using Application.Core.Services;
using Ardalis.Result;
using Xunit;

namespace UnitTests.Core.Services
{
    public class DancerServiceTests
    {
        #region GetDancerByIdAsync Tests
        
        [Fact(DisplayName = "If data source contains dancer id, then return the dancer")]
        public async Task GetDancerByIdAsync_ReturnDancerIfFound()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var dancerGuid = Guid.NewGuid();
            var dancer = new Dancer()
            {
                Id = dancerGuid
            };
            await repository.AddAsync(dancer);
            await repository.SaveChangesAsync();

            var dancerFromDatabase = await service.GetByIdAsync(dancerGuid, CancellationToken.None);
            
            Assert.True(dancerFromDatabase.IsSuccess);
            Assert.Equal(dancerFromDatabase.Value.Id, dancer.Id);
        }

        [Fact(DisplayName = "If data source does not contain dancer id, then return not found result")]
        public async Task GetDancerByIdAsync_ReturnNotFoundIfNotExist()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();
            var service = new DancerService(repository);
            
            var invalidGuid = Guid.NewGuid();
            var dancer = new Dancer()
            {
                Id = Guid.NewGuid()
            };
            await repository.AddAsync(dancer);
            await repository.SaveChangesAsync();

            var dancerFromDatabase = await service.GetByIdAsync(invalidGuid, CancellationToken.None);
            
            Assert.Equal(ResultStatus.NotFound, dancerFromDatabase.Status);
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
                LegacyAuthId = "sample-123",
                DdrName = "Bob"
            };
            
            var updatedDancer = await service.UpdateDancerAsync(request, CancellationToken.None);
            
            Assert.Equal("new-id", updatedDancer.Value.AuthenticationId);
        }

        #endregion
    }
}