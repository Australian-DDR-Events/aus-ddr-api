using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Charts;

namespace Application.Core.Interfaces.Services;

public interface IChartService
{
    Task<bool> CreateChart(CreateChartRequestModel request, CancellationToken cancellationToken);
}
