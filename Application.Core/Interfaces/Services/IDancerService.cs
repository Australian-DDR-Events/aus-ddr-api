using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface IDancerService
    {
        Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<Dancer>> GetDancerByAuthIdAsync(string authId, CancellationToken cancellationToken);

        Task<Result<IList<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken);
        Task<Result<bool>> AddNewDancerAsync(Guid id, string name, string code, string authenticationId, string primaryMachineLocation, string state, CancellationToken cancellationToken);
    }
}