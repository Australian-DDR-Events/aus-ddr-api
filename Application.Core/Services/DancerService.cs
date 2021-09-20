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
    }
}