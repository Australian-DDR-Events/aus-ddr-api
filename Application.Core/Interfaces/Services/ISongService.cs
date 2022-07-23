using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Song;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface ISongService
    {
        IEnumerable<Song> GetSongs(int page, int limit);
        Task CreateSongAsync(CreateSongRequestModel songRequestModel, CancellationToken cancellationToken);
        Result<Song> GetSong(Guid songId);
    }
}