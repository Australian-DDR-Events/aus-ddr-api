using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface IBadgeService
    {
        Task<Result<IList<Badge>>> GetBadgesAsync(int page, int limit, CancellationToken cancellationToken);
    }
}