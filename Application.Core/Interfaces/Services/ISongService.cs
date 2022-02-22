using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface ISongService : ICommonService<Song>
    {
        Task<Result<IList<Song>>> GetSongsAsync(int page, int limit, CancellationToken cancellationToken);
        Task<Result<Song>> CreateSongAsync(Song song, CancellationToken cancellationToken);
        Task<Result<Song>> GetSongWithTopScores(Guid songId, CancellationToken cancellationToken);
    }
}