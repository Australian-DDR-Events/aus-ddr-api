using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces
{
    public interface IDancerService
    {
        Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<IList<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken);
    }
}