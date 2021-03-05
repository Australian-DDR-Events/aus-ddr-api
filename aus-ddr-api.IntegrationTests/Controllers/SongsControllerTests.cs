using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Controllers;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.Song;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace aus_ddr_api.IntegrationTests.Controllers
{
    [Collection("Postgres database collection")]
    public class SongsControllerTests
    {
        private readonly PostgresDatabaseFixture _fixture;

        private readonly ILogger<SongsController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IFileStorage _fileStorage;
        private readonly ISong _songService;

        private readonly SongsController _songsController;

        public SongsControllerTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _logger = new Logger<SongsController>(new NullLoggerFactory());
            _coreDataService = new DbCoreData(_fixture._context);
            _fileStorage = Substitute.For<IFileStorage>();
            _songService = new DbSong(_fixture._context);
            
            _songsController = new SongsController(_logger, _coreDataService, _fileStorage, _songService);
            
            Setup.DropAllRows(_fixture._context);
        }

        [Fact]
        public void Get_WhenDatabaseIsEmpty_Returns_EmptyListOfSongResponses()
        {
            var actionResult = _songsController.Get(null);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Empty(songs);
        }

        [Fact]
        public void Get_WhenDatabaseHasSongs_Returns_ListOfSongResponses()
        {
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

            var actionResult = _songsController.Get(null);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Collection(songs,
                Assert.NotNull, Assert.NotNull
                );
        }

        [Fact]
        public void Get_WhenSongIdsAreProvided_Returns_ListOfSongsIncludedInRequest()
        { 
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

            _fixture._context.Songs.Add(song1);
            var songToGet = _fixture._context.Songs.Add(song2).Entity;
            _fixture._context.SaveChanges();

            var expectedSong = SongResponse.FromEntity(songToGet);

            var actionResult = _songsController.Get(new Guid[]{songToGet.Id});
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Collection(songs,
                song => Assert.Equal(song, expectedSong)
            );
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
            
            var actionResult = _songsController.Get(null);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songs = Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Collection(songs,
                element => Assert.Equal(expectedResponse, element)
                );
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
                Name = "name",
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

        [Fact]
        public async Task Put_GivenSongExists_UpdateExistingSongInDatabase()
        {
            var song = new Song
            {
                Artist = "artist",
                Difficulty = "difficulty",
                Level = 5,
                Name = "name"
            };
            var songEntity = await _fixture._context.Songs.AddAsync(song);
            await _fixture._context.SaveChangesAsync();
            song = songEntity.Entity;

            var expectedSongResponse = new SongResponse
            {
                Artist = "newArtist",
                Difficulty = "difficulty",
                Level = 15,
                Name = "name",
                Id = song.Id
            };

            var actionResult = await _songsController.Put(
                song.Id, 
                new SongRequest
                {
                    Artist = "newArtist",
                    Difficulty = "difficulty",
                    Level = 15,
                    Name = "name",
                });

            var songFromDatabase = await _fixture._context.Songs.FindAsync(song.Id);
            var songFromDatabaseAsResponse = SongResponse.FromEntity(songFromDatabase);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songResponse = Assert.IsAssignableFrom<SongResponse>(okObjectResult.Value);
            Assert.Equal(songFromDatabaseAsResponse, songResponse);
            Assert.Equal(expectedSongResponse, songResponse);
        }

        [Fact]
        public async Task Put_GivenSongDoesNotExist_ReturnNotFoundResponse()
        {
            var actionResult = await _songsController.Put(Guid.NewGuid(), new SongRequest());
            
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Delete_GivenSongExists_DeletesSongFromDatabase()
        {
            var song = new Song
            {
                Artist = "artist",
                Difficulty = "difficulty",
                Level = 5,
                Name = "name"
            };
            var songEntity = await _fixture._context.Songs.AddAsync(song);
            await _fixture._context.SaveChangesAsync();
            song = songEntity.Entity;

            var actionResult = await _songsController.Delete(song.Id);
            
            var songFromDatabase = _fixture._context.Songs.FirstOrDefault(s => s.Id == song.Id);
            
            Assert.IsType<OkResult>(actionResult);
            Assert.Null(songFromDatabase);
        }

        [Fact]
        public async Task Delete_GivenSongDoesNotExist_ReturnNotFoundResponse()
        {
            var actionResult = await _songsController.Delete(Guid.NewGuid());
            
            Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}