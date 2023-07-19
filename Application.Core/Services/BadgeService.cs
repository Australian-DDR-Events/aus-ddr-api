using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Application.Core.Models.Badge;

namespace Application.Core.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;

        public BadgeService(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }
        
        public IEnumerable<GetBadgesResponseModel> GetBadges(int page, int limit)
        {
            var skip = page * limit;
            return _badgeRepository.GetBadges(skip, limit);
        }

        public async Task<Result<Badge>> CreateBadgeAsync(Badge newBadge, CancellationToken cancellationToken)
        {
            var badge = new Badge
            {
                Id = Guid.NewGuid(),
                Description = newBadge.Description,
                Name = newBadge.Name,
                EventId = newBadge.EventId,
                Threshold = newBadge.Threshold
            };
            
            await _badgeRepository.CreateBadge(badge, cancellationToken);
            return new Result<Badge>
            {
                ResultCode =  ResultCode.Ok,
                Value = badge
            };
        }
    }
}