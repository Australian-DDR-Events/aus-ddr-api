using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface ISongRepository
{
    Song? GetSong(Guid songId);
    IEnumerable<Song> GetSongs(int skip, int limit);
    Task CreateSong(Song song, CancellationToken cancellationToken);
}