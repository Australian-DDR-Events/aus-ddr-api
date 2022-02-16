using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Application.Core.Specifications;
using Application.Core.Specifications.DancerSpecs;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class DancerService : CommonService<Dancer>, IDancerService
    {
        private readonly IAsyncRepository<Dancer> _repository;
        private readonly IAsyncRepository<Badge> _badgeRepository;

        public DancerService(IAsyncRepository<Dancer> repository, IAsyncRepository<Badge> badgeRepository) : base(repository)
        {
            _repository = repository;
            _badgeRepository = badgeRepository;
        }

        public async Task<Result<IList<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var dancersSpec = new PageableSpec<Dancer>(skip, limit);
            var dancers = await _repository.ListAsync(dancersSpec, cancellationToken);
            return Result<IList<Dancer>>.Success(dancers);
        }

        public async Task<Result<Dancer>> GetDancerByAuthId(string authId, CancellationToken cancellationToken)
        {
            var byAuthIdSpec = new ByAuthIdSpec(authId);
            var dancer = await _repository.GetBySpecAsync(byAuthIdSpec, cancellationToken);
            return dancer != null ? Result<Dancer>.Success(dancer) : Result<Dancer>.NotFound();
        }

        public async Task<Result<Dancer>> MigrateDancer(MigrateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var dancer = await _repository.GetBySpecAsync(new ByAuthIdSpec(requestModel.AuthId), cancellationToken);
            if (dancer != null) return Result<Dancer>.Success(dancer);
            if (requestModel.LegacyId == null) return Result<Dancer>.NotFound();
            
            dancer = await _repository.GetBySpecAsync(new ByAuthIdSpec(requestModel.LegacyId), cancellationToken);
            if (dancer == null) return Result<Dancer>.NotFound();

            dancer.AuthenticationId = requestModel.AuthId;
            await _repository.SaveChangesAsync(cancellationToken);

            return Result<Dancer>.Success(dancer);
        }

        public async Task<Result<Dancer>> CreateDancerAsync(CreateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var byAuthIdSpec = new ByAuthIdSpec(requestModel.AuthId);
            var dancer = await _repository.GetBySpecAsync(byAuthIdSpec, cancellationToken);
            if (dancer != null)
            {
                return Result<Dancer>.Error("Dancer already exists");
            }

            dancer = new Dancer
            {
                AuthenticationId = requestModel.AuthId,
                DdrCode = requestModel.DdrCode,
                DdrName = requestModel.DdrName,
                State = requestModel.State,
                PrimaryMachineLocation = requestModel.PrimaryMachineLocation
            };

            dancer = await _repository.AddAsync(dancer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return Result<Dancer>.Success(dancer);
        }

        public async Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel, CancellationToken cancellationToken)
        {
            var byAuthIdSpec = new ByAuthIdSpec(requestModel.AuthId);
            var dancer = await _repository.GetBySpecAsync(byAuthIdSpec, cancellationToken);
            if (dancer == null)
            {
                return Result<Dancer>.NotFound();
            }

            dancer.State = requestModel.State;
            dancer.DdrCode = requestModel.DdrCode;
            dancer.DdrName = requestModel.DdrName;
            dancer.PrimaryMachineLocation = requestModel.PrimaryMachineLocation;

            await _repository.SaveChangesAsync(cancellationToken);
            return Result<Dancer>.Success(dancer);
        }

        public async Task<Result<ICollection<Badge>>> GetDancerBadgesAsync(Guid id,
            CancellationToken cancellationToken)
        {
            var dancerBadgesSpec = new DancerBadgesSpec(id);
            var dancer = await _repository.GetBySpecAsync(dancerBadgesSpec, cancellationToken);
            if (dancer == null)
            {
                return Result<ICollection<Badge>>.NotFound();
            }
            return Result<ICollection<Badge>>.Success(dancer.Badges);
        }

        public async Task<Result<bool>> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
        {
            var dancerBadgesSpec = new DancerBadgesSpec(dancerId);
            var dancer = await _repository.GetBySpecAsync(dancerBadgesSpec, cancellationToken);
            var badge = await _badgeRepository.GetByIdAsync(badgeId, cancellationToken);
            if (dancer == null || badge == null)
            {
                return Result<bool>.NotFound();
            }

            dancer.Badges.Add(badge);
            await _repository.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}