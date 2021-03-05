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
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace UnitTests.Controllers
{
    public class SongsControllerTests
    {
        private readonly SongsController _songsController;

        private readonly ILogger<SongsController> _logger;
        private readonly ICoreData _coreDataService;
        private readonly IFileStorage _fileStorage;
        private readonly ISong _songService;
        
        public SongsControllerTests()
        {
            _logger = Substitute.For<ILogger<SongsController>>();
            _songService = Substitute.For<ISong>();
            _fileStorage = Substitute.For<IFileStorage>();
            _coreDataService = Substitute.For<ICoreData>();

            _songsController = new SongsController(_logger, _coreDataService, _fileStorage, _songService);
        }

        [Fact]
        public void Get_ReturnsOkObjectResult_With_EnumerableSongResponse()
        {
            var firstSong = new Song
            {
                Artist = "Artist1",
                Difficulty = "Hard",
                Id = Guid.Parse("A08FE223-A638-48E1-9A9A-78546902C617"),
                Level = 15,
                Name = "Song1"
            };
            var secondSong = new Song
            {
                Artist = "Artist2",
                Difficulty = "Easy",
                Id = Guid.Parse("853B7DCF-3FAA-4D35-886B-0CB3C1A7A2E5"),
                Level = 1,
                Name = "Song2"
            };
            var songList = new List<Song> {firstSong, secondSong};
            _songService.GetAll().Returns(songList);

            var expectedSongList = songList.Select(SongResponse.FromEntity).AsEnumerable();

            var actionResult = _songsController.Get(null);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsAssignableFrom<IEnumerable<SongResponse>>(okObjectResult.Value);
            Assert.Equal(okObjectResult.Value, expectedSongList);
        }

        [Fact]
        public void GetSong_WhenSongNotFound_ReturnsNotFoundResult()
        {
            _songService.Get(Arg.Any<Guid>()).ReturnsNull();

            var actionResult = _songsController.GetSong(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void GetSong_WhenSongIsFound_ReturnsOkObjectResult_With_SongResponse()
        {
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Artist = "Artist1",
                Difficulty = "Difficulty",
                Level = 15,
                Name = "Name1"
            };

            var expectedSongResponse = SongResponse.FromEntity(song);
            _songService.Get(Arg.Is<Guid>(song.Id)).Returns(song);

            var actionResult = _songsController.GetSong(song.Id);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var songResponse = Assert.IsType<SongResponse>(okObjectResult.Value);
            Assert.Equal(expectedSongResponse, songResponse);
        }

        [Fact]
        public async Task Post_ShouldReturnNewSong_With_SongEndpoint()
        {
            var songRequest = new SongRequest
            {
                Artist = "Artist1",
                Difficulty = "Hard",
                Level = 5,
                Name = "Song1"
            };
            
            var songGuid = Guid.Parse("F531D138-44F2-4400-A21B-C3D9A3C8D485");
            var expectedSongResponse = new SongResponse
            {
                Artist = songRequest.Artist,
                Difficulty = songRequest.Difficulty,
                Id = songGuid,
                Level = songRequest.Level,
                Name = songRequest.Name
            };

            _songService.Add(Arg.Any<Song>()).Returns(
                args => Task.FromResult<Song>(new Song
                {
                    Id = songGuid,
                    Artist = args.ArgAt<Song>(0).Artist,
                    Difficulty = args.ArgAt<Song>(0).Difficulty,
                    Level = args.ArgAt<Song>(0).Level,
                    Name = args.ArgAt<Song>(0).Name
                }));

            var actionResult = await _songsController.Post(songRequest);
            
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            Assert.Equal($"/songs/{songGuid}", createdResult.Location);
            var songResponse = Assert.IsType<SongResponse>(createdResult.Value);
            Assert.Equal(expectedSongResponse, songResponse);
            await _coreDataService.Received(1).SaveChanges();
        }

        [Fact]
        public void Delete_GivenSongWasNotDeleted_Should_ReturnNotFound()
        {
            _songService.Delete(Arg.Any<Guid>()).Returns(false);

            var actionResult = _songsController.Delete(Guid.NewGuid());
            
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void Delete_GivenSongWasDeleted_Should_ReturnOkResult()
        {
            _songService.Delete(Arg.Any<Guid>()).Returns(true);

            var actionResult = _songsController.Delete(Guid.NewGuid());
            
            Assert.IsType<OkResult>(actionResult.Result);
            _coreDataService.Received(1).SaveChanges();
        }
    }
}
