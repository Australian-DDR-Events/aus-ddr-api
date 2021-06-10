using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Specifications;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class DancerService : IDancerService
    {
        private readonly IAsyncRepository<Dancer> _repository;

        public DancerService(IAsyncRepository<Dancer> repository)
        {
            _repository = repository;
        }
        
        public async Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var dancerSpec = new DancerByIdSpec(id);
            var dancer = await _repository.GetBySpecAsync(dancerSpec, cancellationToken);
            return dancer == null ? Result<Dancer>.NotFound() : Result<Dancer>.Success(dancer);
        }
    }
}