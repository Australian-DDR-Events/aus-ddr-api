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

        public DancerService(IAsyncRepository<Dancer> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Result<IList<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var dancersSpec = new PageableSpec<Dancer>(skip, limit);
            var dancers = await _repository.ListAsync(dancersSpec, cancellationToken);
            return Result<IList<Dancer>>.Success(dancers);
        }

        public async Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel, CancellationToken cancellationToken)
        {
            var byAuthIdSpec = new ByAuthIdSpec(requestModel.AuthId);
            var byLegacyAuthIdSpec = new ByAuthIdSpec(requestModel.LegacyAuthId);
            var dancer = await _repository.GetBySpecAsync(byAuthIdSpec, cancellationToken);
            if (dancer == null && requestModel.LegacyAuthId != null) dancer = await _repository.GetBySpecAsync(byLegacyAuthIdSpec, cancellationToken);
            dancer ??= new Dancer();

            dancer = new Dancer
            {
                Id = dancer.Id,
                AuthenticationId = requestModel.AuthId,
                DdrCode = requestModel.DdrCode,
                DdrName = requestModel.DdrName,
                State = requestModel.State,
                PrimaryMachineLocation = requestModel.PrimaryMachineLocation
            };

            if (dancer.Id == Guid.Empty)
            {
                await _repository.AddAsync(dancer, cancellationToken);
            }

            await _repository.SaveChangesAsync(cancellationToken);
            return Result<Dancer>.Success(dancer);
        }
    }
}