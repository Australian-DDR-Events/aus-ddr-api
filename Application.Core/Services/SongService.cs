using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Specifications.SongSpecs;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class SongService : CommonService<Song>, ISongService
    {
        private readonly IAsyncRepository<Song> _repository;

        public SongService(IAsyncRepository<Song> repository) : base(repository)
        {
            _repository = repository;
        }
        public async Task<Result<IList<Song>>> GetSongsAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var songsSpec = new List(skip, limit);
            var songs = await _repository.ListAsync(songsSpec, cancellationToken);
            return Result<IList<Song>>.Success(songs);
        }
    }
}