using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces
{
    public interface IDancerService
    {
        Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}