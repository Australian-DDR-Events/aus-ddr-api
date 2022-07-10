using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Dancer;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services
{
    public interface IDancerService
    {
        // Task<Result<Dancer>> GetDancerByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<IEnumerable<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken);

        Result<Dancer> GetDancerById(Guid id);

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

        Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel,
            CancellationToken cancellationToken);
            
        Result<IEnumerable<GetDancerBadgesResponseModel>> GetDancerBadges(Guid id);

        Task<Result<bool>> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken);
        Task<Result<bool>> RemoveBadgeFromDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken);

        Task<Result<bool>> SetAvatarForDancerByAuthId(string authId, Stream fileStream, CancellationToken cancellationToken);
    }
}