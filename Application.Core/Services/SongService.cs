using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Specifications.SongSpecs;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class SongService : CommonService<Song>, ISongService
    {
        private readonly IAsyncRepository<Song> _repository;
        private readonly ISongRepository _songRepository;

        public SongService(IAsyncRepository<Song> repository, ISongRepository songRepository) : base(repository)
        {
            _repository = repository;
            _songRepository = songRepository;
        }
        public IEnumerable<Song> GetSongs(int page, int limit)
        {
            var skip = page * limit;
            return _songRepository.GetSongs(skip, limit);
        }

        public async Task<Result<Song>> CreateSongAsync(Song song, CancellationToken cancellationToken)
        {
            var created = await _repository.AddAsync(song, cancellationToken);
            return Result<Song>.Success(created);
        }

        public Result<Song> GetSong(Guid songId, bool withTopScores)
        {
            var result = _songRepository.GetSong(songId, withTopScores);
            return result == null ? Result<Song>.NotFound() : Result<Song>.Success(result);
        }
    }
}