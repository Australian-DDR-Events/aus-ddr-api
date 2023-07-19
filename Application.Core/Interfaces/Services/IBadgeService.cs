using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models;
using Application.Core.Models.Badge;

namespace Application.Core.Interfaces.Services
{
    public interface IBadgeService
    {
        IEnumerable<GetBadgesResponseModel> GetBadges(int page, int limit);
        Task<Result<Badge>> CreateBadgeAsync(Badge badge, CancellationToken cancellationToken);
    }
}