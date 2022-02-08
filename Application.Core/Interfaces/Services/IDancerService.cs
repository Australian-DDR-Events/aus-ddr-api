using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Dancer;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface IDancerService : ICommonService<Dancer>
    {
        // Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<IList<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken);

        Task<Result<Dancer>> GetDancerByAuthId(string authId, CancellationToken cancellationToken);

        Task<Result<Dancer>> CreateDancerAsync(CreateDancerRequestModel requestModel,
            CancellationToken cancellationToken);

        /// <summary>
        /// Given an auth id and legacy auth id, updates the database
        /// with the auth id given the legacy auth id is currently
        /// in use.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<Dancer>> MigrateDancer(MigrateDancerRequestModel requestModel, CancellationToken cancellationToken);

        Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel, CancellationToken cancellationToken);
    }
}