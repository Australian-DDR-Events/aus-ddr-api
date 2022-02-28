using System;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Infrastructure.Data;

public class SongRepository : ISongRepository
{
    private readonly EFDatabaseContext _context;
    
    public SongRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public Song GetSongWithTopScores(Guid songId)
    {
        return _context
            .Songs
            .Where(s => s.Id.Equals(songId))
            .Select(s => new Song
            {
                Id = s.Id,
                Name = s.Name,
                Artist = s.Artist,
                SongDifficulties = s.SongDifficulties.Select(sd => new SongDifficulty
                {
                    Id = sd.Id,
                    Difficulty = sd.Difficulty,
                    Level = sd.Level,
                    PlayMode = sd.PlayMode,
                    Scores = sd.Scores.OrderByDescending(score => score.Value).Take(3).ToArray()
                }).ToList()
            }).FirstOrDefault();
    }
}