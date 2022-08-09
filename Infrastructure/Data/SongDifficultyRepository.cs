using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class SongDifficultyRepository : ISongDifficultyRepository
{
    private readonly EFDatabaseContext _context;
    
    public SongDifficultyRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateSongDifficulty(Guid songId, SongDifficulty songDifficulty, CancellationToken cancellationToken)
    {
        var song = _context.Songs.FirstOrDefault(s => s.Id.Equals(songId));
        if (song == null) return false;
        songDifficulty.SongId = songId;
        await _context
            .SongDifficulties
            .AddAsync(songDifficulty, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
