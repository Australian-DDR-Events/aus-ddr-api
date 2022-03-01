using System;
using Application.Core.Entities;
using JetBrains.Annotations;

namespace Application.Core.Interfaces.Repositories;

public interface ISongRepository
{
    Song? GetSong(Guid songId, bool withTopScores);
}