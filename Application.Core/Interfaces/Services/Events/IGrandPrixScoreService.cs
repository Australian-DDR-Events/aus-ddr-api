using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Score;

namespace Application.Core.Interfaces.Services.Events;

public interface IGrandPrixScoreService
{
    public Task<bool> CreateScoreAsync(CreateScoreRequestModel requestModel, CancellationToken cancellationToken);
}