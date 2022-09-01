using System;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IScoreRepository
{
    public Task<bool> CreateScore(Guid songDifficultyId, Guid dancerId, Score score, Guid? eventId);
}
