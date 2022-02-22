using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Services;
using Infrastructure.Data;
using Moq;
using Xunit;

namespace UnitTests.Core.Services
{
    [Collection("Postgres database collection")]
    public class SongServiceTests
    {
        #region GetSongsAsync Tests
        
        [Fact(DisplayName = "If songs contain difficulties, returns songs with difficulties")]
        public async Task GetSongsAsync_ReturnSongsWithDifficulties()
        {
            var repository = InMemoryDatabaseRepository<Song>.CreateRepository();
            var songRepository = new Mock<SongRepository>();
            var service = new SongService(repository, songRepository.Object);
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
            Task.WaitAll(songs.Select(s => repository.AddAsync(s)).ToArray());
            await repository.SaveChangesAsync();

            var songsFromDatabase = await service.GetSongsAsync(0, 2, CancellationToken.None);
            
            Assert.True(songsFromDatabase.IsSuccess);
            Assert.Equal(songs.OrderBy(d => d.Id), songsFromDatabase.Value);
        }
        
        [Fact(DisplayName = "If song count exceeds paging size, only take number of songs required")]
        public async Task GetSongsAsync_OnlyTakeSongsWithinPageSize()
        {
            var repository = InMemoryDatabaseRepository<Song>.CreateRepository();
            var songRepository = new Mock<SongRepository>();
            var service = new SongService(repository, songRepository.Object);
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
            Task.WaitAll(songs.Select(s => repository.AddAsync(s)).ToArray());
            await repository.SaveChangesAsync();

            var songsFromDatabase = await service.GetSongsAsync(1, 2, CancellationToken.None);
            
            Assert.True(songsFromDatabase.IsSuccess);
            Assert.Equal(2, songsFromDatabase.Value.Count);
            Assert.Equal(songs.OrderBy(d => d.Id).Skip(2).Take(2), songsFromDatabase.Value);
        }
        
        #endregion
    }
}