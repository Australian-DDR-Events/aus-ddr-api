using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Services;
using Ardalis.Result;
using Infrastructure.Data;
using Xunit;

namespace IntegrationTests.Core.Services
{
    [Collection("Postgres database collection")]
    public class DancerServiceTests
    {
        private readonly PostgresDatabaseFixture _fixture;
        private readonly IAsyncRepository<Dancer> _dancerRepository;
        private readonly DancerService _dancerService;

        public DancerServiceTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _dancerRepository = new GenericEfRepository<Dancer>(_fixture._context);
            _dancerService = new DancerService(_dancerRepository);
            
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

            var dancerFromDatabase = await _dancerService.GetDancerByIdAsync(dancerGuid, CancellationToken.None);
            
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

            var dancerFromDatabase = await _dancerService.GetDancerByIdAsync(invalidGuid, CancellationToken.None);
            
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
    }
}