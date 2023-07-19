using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Interfaces.Services.Events;
using Application.Core.Models.Score;

namespace Application.Core.Services.EventImplementations.GrandPrix;

public class GrandPrixScoreService : IGrandPrixScoreService
{
    private IScoreService _scoreService;

    public GrandPrixScoreService(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    
    public Task<bool> CreateScoreAsync(CreateScoreRequestModel requestModel, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}