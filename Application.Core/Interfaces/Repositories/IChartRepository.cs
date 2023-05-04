using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IChartRepository
{
    Task<bool> CreateChart(Guid songId, Chart chart, CancellationToken cancellationToken);
}