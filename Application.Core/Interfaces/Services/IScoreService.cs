using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Score;

namespace Application.Core.Interfaces.Services;

public interface IScoreService
{
    public Task<bool> CreateScoreAsync(CreateScoreRequestModel requestModel, CancellationToken cancellationToken);
}