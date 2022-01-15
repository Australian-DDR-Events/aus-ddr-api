using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Specifications;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IAsyncRepository<Badge> _repository;

        public BadgeService(IAsyncRepository<Badge> repository)
        {
            _repository = repository;
        }
        
        public async Task<Result<IList<Badge>>> GetBadgesAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var badgesSpec = new PageableSpec<Badge>(skip, limit);
            var badges = await _repository.ListAsync(badgesSpec, cancellationToken);
            return Result<IList<Badge>>.Success(badges);
        }

        public async Task<Result<Badge>> CreateBadgeAsync(Badge newBadge, CancellationToken cancellationToken)
        {
            var badge = await _repository.AddAsync(newBadge, cancellationToken);
            return Result<Badge>.Success(badge);
        }
    }
}