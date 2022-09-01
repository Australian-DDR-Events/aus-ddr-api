using System;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class ScoreRepository : IScoreRepository
{
    private readonly EFDatabaseContext _context;
    
    public ScoreRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateScore(Guid songDifficultyId, Guid dancerId, Score score, Guid? eventId)
    {
        var databaseEntry = new Score
        {
            Id = score.Id,
            ExScore = score.ExScore,
            Value = score.Value,
            SubmissionTime = DateTime.Now,
            SongDifficultyId = songDifficultyId,
            EventId = eventId,
            DancerId = dancerId
        };

        try
        {
            await _context.Scores.AddAsync(databaseEntry);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}