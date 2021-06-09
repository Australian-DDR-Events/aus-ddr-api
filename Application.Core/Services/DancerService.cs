using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Specifications;

namespace Application.Core.Services
{
    public class DancerService : IDancerService
    {
        private readonly IAsyncRepository<Dancer> _repository;

        public DancerService(IAsyncRepository<Dancer> repository)
        {
            _repository = repository;
        }
        
        public Task<Dancer?> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var dancerSpec = new DancerByIdSpec(id);
            return _repository.GetBySpecAsync(dancerSpec, cancellationToken);
        }
    }
}