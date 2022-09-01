using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.SongDifficulties;

namespace Application.Core.Services;

public class SongDifficultyService : ISongDifficultyService
{
    private readonly ISongDifficultyRepository _songDifficultyRepository;

    public SongDifficultyService(ISongDifficultyRepository songDifficultyRepository)
    {
        _songDifficultyRepository = songDifficultyRepository;
    }
    
    public Task<bool> CreateSongDifficulty(CreateSongDifficultyRequestModel request, CancellationToken cancellationToken)
    {
        var songDifficulty = new SongDifficulty
        {
            Id = Guid.NewGuid(),
            Difficulty = request.Difficulty,
            PlayMode = request.Mode,
            Level = request.Level,
            MaxScore = request.MaxScore
        };

        return _songDifficultyRepository.CreateSongDifficulty(request.SongId, songDifficulty, cancellationToken);
    }
}