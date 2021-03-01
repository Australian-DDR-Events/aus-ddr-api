using System;
using System.Collections.Generic;
using AusDdrApi.Controllers;
using AusDdrApi.Entities;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.Score;
using AusDdrApi.Services.Song;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace aus_ddr_api.IntegrationTests.Controllers
{   
    [Collection("Postgres database collection")]
    public class ScoresControllerTests
    {
        private readonly PostgresDatabaseFixture _fixture;

        private readonly ILogger<ScoresController> _logger;
        private readonly ICoreData _coreService;
        private readonly IScore _scoreService;
        private readonly ISong _songService;
        private readonly IDancer _dancerService;
        private readonly IFileStorage _fileStorage;

        private readonly ScoresController _scoresController;

        public ScoresControllerTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _logger = new Logger<ScoresController>(new NullLoggerFactory());
            _coreService = new DbCoreData(_fixture._context);
            _scoreService = new DbScore(_fixture._context);
            _songService = new DbSong(_fixture._context);
            _dancerService = new DbDancer(_fixture._context);
            _fileStorage = new LocalFileStorage(".");
            
            _scoresController = new ScoresController(
                _logger,
                _coreService,
                _scoreService,
                _songService,
                _dancerService,
                _fileStorage);
            
            Setup.DropAllRows(_fixture._context);
        }

        [Fact]
        public void GetScore_Given_ScoreExists_Return_ActionResultWithScoreResponse()
        {
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Name = "song1",
                Artist = "artist1",
                Difficulty = "Hard",
                Level = 12
            };
            _fixture._context.Songs.Add(song);
            _fixture._context.SaveChanges();

        }
    }
}