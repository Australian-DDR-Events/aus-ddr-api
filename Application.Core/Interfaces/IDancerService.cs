using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces
{
    public interface IDancerService
    {
        Task<Dancer?> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}