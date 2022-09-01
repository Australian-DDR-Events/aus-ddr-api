using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Song;
using Application.Core.Models.SongDifficulties;

namespace Application.Core.Interfaces.Services;

public interface ISongDifficultyService
{
    Task<bool> CreateSongDifficulty(CreateSongDifficultyRequestModel request, CancellationToken cancellationToken);
}
