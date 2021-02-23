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
using NSubstitute;
using NUnit.Framework;

namespace aus_ddr_api.UnitTests.Controllers
{
    [TestFixture]
    public class SongsController_Tests
    {
        private SongsController _songsController;

        private ILogger<SongsController> _logger;
        private ICoreData _coreDataService;
        private ISong _songService;
        
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<SongsController>>();
            _songService = Substitute.For<ISong>();
            _coreDataService = Substitute.For<ICoreData>();

            _songsController = new SongsController(_logger, _coreDataService, _songService);
        }

        [Test]
        public void Get_ReturnsAllSongs()
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

            var actionResult = _songsController.Get();

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            
            Assert.AreEqual(expectedSongList, result.Value);
        }
    }
}