using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Charts;

namespace Application.Core.Services;

public class ChartService : IChartService
{
    private readonly IChartRepository _chartRepository;

    public ChartService(IChartRepository chartRepository)
    {
        _chartRepository = chartRepository;
    }
    
    public Task<bool> CreateChart(CreateChartRequestModel request, CancellationToken cancellationToken)
    {
        var chart = new Chart
        {
            Id = Guid.NewGuid(),
            Difficulty = request.Difficulty,
            PlayMode = request.Mode,
            Level = request.Level,
            MaxScore = request.MaxScore
        };

        return _chartRepository.CreateChart(request.SongId, chart, cancellationToken);
    }
}