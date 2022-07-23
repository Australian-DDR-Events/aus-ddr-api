using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Badge;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IAsyncRepository<Badge> _repository;

        public BadgeService(IBadgeRepository badgeRepository, IAsyncRepository<Badge> repository)
        {
            _badgeRepository = badgeRepository;
            _repository = repository;
        }
        
        public IEnumerable<GetBadgesResponseModel> GetBadges(int page, int limit)
        {
            var skip = page * limit;
            return _badgeRepository.GetBadges(skip, limit);
        }

        public async Task<Result<Badge>> CreateBadgeAsync(Badge newBadge, CancellationToken cancellationToken)
        {
            var badge = await _repository.AddAsync(newBadge, cancellationToken);
            return Result<Badge>.Success(badge);
        }
    }
}