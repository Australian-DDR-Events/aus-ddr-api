using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Score;

namespace Application.Core.Services;

public class ScoreService : IScoreService
{
    private readonly IChartRepository _chartRepository;
    private readonly IScoreRepository _scoreRepository;
    
    public ScoreService(IChartRepository chartRepository, IScoreRepository scoreRepository)
    {
        _chartRepository = chartRepository;
        _scoreRepository = scoreRepository;
    }

    public async Task<bool> CreateScoreAsync(CreateScoreRequestModel requestModel, CancellationToken cancellationToken)
    {
        var chart = _chartRepository.GetChartById(requestModel.ChartId);
        if (chart == null)
        {
            return false;
        }
        
        var score = new Score()
        {
            ExScore = requestModel.ExScore,
            Value = requestModel.Score,
        };
        await _scoreRepository.CreateScore(requestModel.ChartId, requestModel.DancerId, score, null);
        return true;
    }
}