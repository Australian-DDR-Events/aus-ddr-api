using System;
using System.Collections.Generic;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface ISongRepository
{
    Song? GetSong(Guid songId, bool withTopScores);
    IEnumerable<Song> GetSongs(int skip, int limit);
}