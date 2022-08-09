using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface ISongDifficultyRepository
{
    Task<bool> CreateSongDifficulty(Guid songId, SongDifficulty songDifficulty, CancellationToken cancellationToken);
}