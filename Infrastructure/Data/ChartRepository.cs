using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Infrastructure.Data;

public class ChartRepository : IChartRepository
{
    private readonly EFDatabaseContext _context;
    
    public ChartRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateChart(Guid songId, Chart chart, CancellationToken cancellationToken)
    {
        var song = _context.Songs.FirstOrDefault(s => s.Id.Equals(songId));
        if (song == null) return false;
        chart.SongId = songId;
        await _context
            .Charts
            .AddAsync(chart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Chart GetChartById(Guid chartId)
    {
        return _context
            .Charts
            .FirstOrDefault(c => c.Id.Equals(chartId));
    }
}
