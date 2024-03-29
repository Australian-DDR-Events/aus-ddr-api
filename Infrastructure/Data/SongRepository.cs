using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class SongRepository : ISongRepository
{
    private readonly EFDatabaseContext _context;
    
    public SongRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public Song GetSong(Guid songId)
    {
        return _context
            .Songs
            .Where(s => s.Id.Equals(songId))
            .Select(s => new Song
            {
                Id = s.Id,
                Name = s.Name,
                Artist = s.Artist,
                Charts = s.Charts.Select(sd => new Chart
                {
                    Id = sd.Id,
                    Difficulty = sd.Difficulty,
                    Level = sd.Level,
                    PlayMode = sd.PlayMode
                }).ToList()
            }).FirstOrDefault();
    }
    
    public IEnumerable<Song> GetSongs(int skip, int limit)
    {
        return _context
            .Songs
            .OrderBy(s => s.Name)
            .Skip(skip)
            .Take(limit)
            .Select(s => new Song
            {
                Id = s.Id,
                Name = s.Name,
                Artist = s.Artist
            })
            .ToList();
    }

    public async Task CreateSong(Song song, CancellationToken cancellationToken)
    {
        _context
            .Songs
            .Add(song);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
