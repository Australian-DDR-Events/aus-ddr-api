using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Song;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }
        public IEnumerable<Song> GetSongs(int page, int limit)
        {
            var skip = page * limit;
            return _songRepository.GetSongs(skip, limit);
        }

        public async Task CreateSongAsync(CreateSongRequestModel songRequestModel, CancellationToken cancellationToken)
        {
            var song = new Song
            {
                Id = Guid.NewGuid(),
                Name = songRequestModel.Name,
                Artist = songRequestModel.Artist,
                KonamiId = songRequestModel.KonamiId
            };
            await _songRepository.CreateSong(song, cancellationToken);
        }

        public Result<Song> GetSong(Guid songId)
        {
            var result = _songRepository.GetSong(songId);
            return result == null ? Result<Song>.NotFound() : Result<Song>.Success(result);
        }
    }
}