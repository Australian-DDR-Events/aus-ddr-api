using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Services;
using Infrastructure.Data;
using Xunit;

namespace IntegrationTests.Core.Services
{
    [Collection("Postgres database collection")]
    public class SongServiceTests
    {
        private readonly PostgresDatabaseFixture _fixture;
        private readonly IAsyncRepository<Song> _songRepository;
        private readonly ISongService _songService;

        public SongServiceTests(PostgresDatabaseFixture fixture)
        {
            _fixture = fixture;

            _songRepository = new GenericEfRepository<Song>(_fixture._context);
            _songService = new SongService(_songRepository, new SongRepository(_fixture._context));
            
            Setup.DropAllRows(_fixture._context);
        }
        #region GetSongsAsync Tests
        
        [Fact(DisplayName = "If songs contain difficulties, returns songs with difficulties")]
        public async Task GetSongsAsync_ReturnSongsWithDifficulties()
        {
            var song1 = new Song
            {
                Id = Guid.NewGuid(),
                SongDifficulties = new List<SongDifficulty> {
                    new()
                    {
                        Id = Guid.NewGuid()
                    },
                    new()
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };
            var song2 = new Song
            {
                Id = Guid.NewGuid(),
                SongDifficulties = new List<SongDifficulty> {
                    new()
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            var songs = new List<Song>
            {
                song1,
                song2
            };
            await _fixture._context.Songs.AddRangeAsync(songs);
            await _fixture._context.SaveChangesAsync();

            var songsFromDatabase = await _songService.GetSongsAsync(0, 2, CancellationToken.None);
            
            Assert.True(songsFromDatabase.IsSuccess);
            Assert.Equal(songs.OrderBy(d => d.Id), songsFromDatabase.Value);
        }
        
        [Fact(DisplayName = "If song count exceeds paging size, only take number of songs required")]
        public async Task GetSongsAsync_OnlyTakeSongsWithinPageSize()
        {
            var songs = new List<Song>
            {
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid()
                }
            };
            await _fixture._context.Songs.AddRangeAsync(songs);
            await _fixture._context.SaveChangesAsync();

            var songsFromDatabase = await _songService.GetSongsAsync(1, 2, CancellationToken.None);
            
            Assert.True(songsFromDatabase.IsSuccess);
            Assert.Equal(2, songsFromDatabase.Value.Count);
            Assert.Equal(songs.OrderBy(d => d.Id).Skip(2).Take(2), songsFromDatabase.Value);
        }
        
        #endregion
    }
}