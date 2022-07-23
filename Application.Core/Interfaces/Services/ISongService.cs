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
        IEnumerable<Song> GetSongs(int page, int limit);
        Task<Result<Song>> CreateSongAsync(Song song, CancellationToken cancellationToken);
        Result<Song> GetSong(Guid songId, bool withTopScores);
    }
}