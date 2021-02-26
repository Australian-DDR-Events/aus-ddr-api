using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Controllers;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
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

        [Fact]
        public void Get_WhenDatabaseHasSongs_Returns_ListOfSongResponses_With_CorrectData()
        {
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Artist = "artist",
                Level = 1,
                Difficulty = "ESP",
                Name = "name"
            };
            var expectedResponse = new SongResponse
            {
                Artist = song.Artist,
                Difficulty = song.Difficulty,
                Id = song.Id,
                Level = song.Level,
                Name = song.Name
            };
            
            _fixture._context.Songs.Add(song);
            _fixture._context.SaveChanges();
            
            var actionResult = _songsController.Get();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Equal(1, songs.Count());
            Assert.Equal(expectedResponse, songs.ElementAt(0));
        }

        [Fact]
        public void GetSong_When_SongDoesNotExist_Returns_NotFoundActionResult()
        {
            var actionResult = _songsController.GetSong(Guid.NewGuid());

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void GetSong_When_SongExists_Returns_SongResponse()
        {           
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Artist = "artist",
                Level = 1,
                Difficulty = "ESP",
                Name = "name"
            };
            var expectedResponse = new SongResponse
            {
                Artist = song.Artist,
                Difficulty = song.Difficulty,
                Id = song.Id,
                Level = song.Level,
                Name = song.Name
            };
            
            _fixture._context.Songs.Add(song);
            _fixture._context.SaveChanges();

            var actionResult = _songsController.GetSong(song.Id);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songResponse = Assert.IsAssignableFrom<SongResponse>(okObjectResult.Value);
            Assert.Equal(expectedResponse, songResponse);
        }

        [Fact]
        public async Task Post_GivenSongDoesNotExist_CreatesSongInDatabase()
        {
            var songRequest = new SongRequest
            {
                Artist = "artist",
                Difficulty = "difficulty",
                Level = 5,
                Name = "name"
            };
            
            var expectedSong = new Song
            {
                Artist = songRequest.Artist,
                Difficulty = songRequest.Difficulty,
                Level = songRequest.Level,
                Name = songRequest.Name
            };

            var actionResult = await _songsController.Post(songRequest);
            
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            var id = Guid.Parse(createdResult.Location.Substring(createdResult.Location.LastIndexOf("/") + 1));
            expectedSong.Id = id;

            var newSong = await _fixture._context.Songs.FindAsync(id);
            Assert.Equal(expectedSong, newSong);
        }
    }
}