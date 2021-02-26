using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Controllers;
using AusDdrApi.Entities;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Song;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace aus_ddr_api.IntegrationTests.Controllers
{
    [Collection("Postgres database collection")]
    public class SongsControllerTests
    {
        private readonly PostgresDatabaseFixture _fixture;

        private readonly ILogger<SongsController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly ISong _songService;

        private readonly SongsController _songsController;

        public SongsControllerTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _logger = new Logger<SongsController>(new NullLoggerFactory());
            _coreDataService = new DbCoreData(_fixture._context);
            _songService = new DbSong(_fixture._context);
            
            _songsController = new SongsController(_logger, _coreDataService, _songService);
        }

        [Fact]
        public void Get_WhenDatabaseIsEmpty_Returns_EmptyListOfSongResponses()
        {
            Setup.DropAllRows(_fixture._context);

            var actionResult = _songsController.Get();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Empty(songs);
        }

        [Fact]
        public void Get_WhenDatabaseHasSongs_Returns_ListOfSongResponses()
        {
            Setup.DropAllRows(_fixture._context);

            var song1 = new Song
            {
                Id = Guid.NewGuid(),
                Level = 1
            };
            var song2 = new Song
            {
                Id = Guid.NewGuid(),
                Level = 1
            };

            _fixture._context.Songs.AddRange(song1, song2);
            _fixture._context.SaveChanges();

            var actionResult = _songsController.Get();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Equal(2, songs.Count());
        }
    }
}