using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Controllers;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.Badges;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace aus_ddr_api.IntegrationTests.Controllers
{
    [Collection("Postgres database collection")]
    public class DancersControllerTests
    {
        private readonly PostgresDatabaseFixture _fixture;
        
        private readonly ILogger<DancersController> _logger;
        private readonly ICoreData _coreService;
        private readonly IDancerService _dancerService;
        private readonly IBadge _badgeService;
        private readonly IFileStorage _fileStorage;
        private readonly IAuthorization _authorizationService;
        
        private readonly DancersController _dancersController;

        public DancersControllerTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _logger = new Logger<DancersController>(new NullLoggerFactory());
            _coreService = new DbCoreData(_fixture._context);
            _dancerService = new DancerService(_fixture._context);
            _badgeService = new DbBadge(_fixture._context);
            _fileStorage = new LocalFileStorage(".");
            _authorizationService = Substitute.For<IAuthorization>();
            
            _dancersController = new DancersController(
                _logger,
                _coreService,
                _dancerService,
                _badgeService,
                _fileStorage,
                _authorizationService);
            
            Setup.DropAllRows(_fixture._context);
        }

        [Fact]
        public void Get_ReturnsActionResult_With_ListOfDancerResponses()
        {
            var dancers = new List<Dancer>
            {
                new Dancer
                {
                    AuthenticationId = "auth_id_1",
                    Id = Guid.NewGuid()
                },
                new Dancer
                {
                    AuthenticationId = "auth_id_2",
                    Id = Guid.NewGuid()
                },
            };
            _fixture._context.Dancers.AddRange(dancers);
            _fixture._context.SaveChanges();

            var actionResult = _dancersController.Get();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dancerResponses = Assert.IsAssignableFrom<IEnumerable<DancerResponse>>(okObjectResult.Value);
            Assert.Collection(dancerResponses,
                Assert.NotNull, Assert.NotNull
            );
        }

        [Fact]
        public void GetDancer_GivenAuthIdExists_ReturnsActionResultWithDancerResponse()
        {
            var dancer = new Dancer
            {
                AuthenticationId = "auth_id_1"
            };
            dancer = _fixture._context.Dancers.Add(dancer).Entity;
            _fixture._context.SaveChanges();

            var expectedDancerResponse = new DancerResponse
            {
                AuthenticationId = dancer.AuthenticationId,
                Id = dancer.Id
            };

            var actionResult = _dancersController.GetDancer(dancer.AuthenticationId);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dancerResponse = Assert.IsAssignableFrom<DancerResponse>(okObjectResult.Value);
            Assert.Equal(expectedDancerResponse, dancerResponse);
        }

        [Fact]
        public void GetDancer_GivenUserIdExists_ReturnsActionResultWithDancerResponse()
        {
            var dancer = new Dancer
            {
                AuthenticationId = "auth_id_1"
            };
            dancer = _fixture._context.Dancers.Add(dancer).Entity;
            _fixture._context.SaveChanges();

            var expectedDancerResponse = new DancerResponse
            {
                AuthenticationId = dancer.AuthenticationId,
                Id = dancer.Id
            };

            var actionResult = _dancersController.GetDancer(dancer.Id.ToString());
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dancerResponse = Assert.IsAssignableFrom<DancerResponse>(okObjectResult.Value);
            Assert.Equal(expectedDancerResponse, dancerResponse);
        }

        [Fact]
        public void GetDancer_GivenUserDoesNotExist_ReturnsActionResultWithDancerResponse()
        {
            var dancer = new Dancer
            {
                AuthenticationId = "auth_id_1"
            };
            dancer = _fixture._context.Dancers.Add(dancer).Entity;
            _fixture._context.SaveChanges();

            var expectedDancerResponse = new DancerResponse
            {
                AuthenticationId = dancer.AuthenticationId,
                Id = dancer.Id
            };

            var actionResult = _dancersController.GetDancer("invalid_id");
            
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Post_GivenUserDoesNotExist_Returns_CreatedActionResultWithNewUser()
        {
            _authorizationService.GetUserId().Returns("user_id");
            var dancerRequest = new DancerRequest();

            var expectedDancerResponse = new DancerResponse
            {
                AuthenticationId = "user_id",
                DdrCode = "573",
                State = "n/a"
            };
            
            var actionResult = await _dancersController.Post(dancerRequest);
            
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            var id = Guid.Parse(createdResult.Location.Substring(createdResult.Location.LastIndexOf("/") + 1));
            expectedDancerResponse.Id = id;
            var dancerResponse = Assert.IsAssignableFrom<DancerResponse>(createdResult.Value);
            Assert.Equal(expectedDancerResponse, dancerResponse);
        }

        [Fact]
        public async Task Post_GivenUserIdExists_Returns_ConflictActionResult()
        {
            _authorizationService.GetUserId().Returns("user_id");
            await _fixture._context.Dancers.AddAsync(new Dancer {AuthenticationId = "user_id"});
            await _fixture._context.SaveChangesAsync();
            
            var actionResult = await _dancersController.Post(new DancerRequest());

            Assert.IsType<ConflictResult>(actionResult.Result);
        }
    }
}