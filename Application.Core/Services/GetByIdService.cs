using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Specifications;
using Ardalis.Result;

namespace Application.Core.Services
{
    public class CommonService<T> : ICommonService<T> where T : BaseEntity, IAggregateRoot
    {
        private readonly IAsyncRepository<T> _repository;

        public CommonService(IAsyncRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<Result<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var spec = new ByIdSpec<T>(id);
            var dancer = await _repository.GetBySpecAsync(spec, cancellationToken);
            return dancer == null ? Result<T>.NotFound() : Result<T>.Success(dancer);
        }
    }
}