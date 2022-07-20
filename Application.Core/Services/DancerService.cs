using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Dancer;
using Application.Core.Specifications.DancerSpecs;
using Ardalis.Result;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Core.Services
{
    public class DancerService : CommonService<Dancer>, IDancerService
    {
        private readonly IAsyncRepository<Dancer> _repository;
        private readonly IAsyncRepository<Badge> _badgeRepository;
        private readonly IDancerRepository _dancerRepository;
        private readonly IFileStorage _fileStorage;

        public DancerService(IAsyncRepository<Dancer> repository, IAsyncRepository<Badge> badgeRepository, IDancerRepository dancerRepository, IFileStorage fileStorage) : base(repository)
        {
            _repository = repository;
            _badgeRepository = badgeRepository;
            _dancerRepository = dancerRepository;
            _fileStorage = fileStorage;
        }

        public async Task<Result<IEnumerable<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var dancers = _dancerRepository.GetDancers(skip, limit);
            return Result<IEnumerable<Dancer>>.Success(dancers);
        }

        public Result<Dancer> GetDancerById(Guid id)
        {
            var dancer = _dancerRepository.GetDancerById(id);
            return dancer != null ? Result<Dancer>.Success(dancer) : Result<Dancer>.NotFound();
        }

        public async Task<Result<Dancer>> MigrateDancer(MigrateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer != null) return Result<Dancer>.Success(dancer);
            if (requestModel.LegacyId == null) return Result<Dancer>.NotFound();
            
            dancer = _dancerRepository.GetDancerByAuthId(requestModel.LegacyId);
            if (dancer == null) return Result<Dancer>.NotFound();

            dancer.AuthenticationId = requestModel.AuthId;
            await _dancerRepository.UpdateDancer(dancer, cancellationToken);

            return Result<Dancer>.Success(dancer);
        }

        public async Task<Result<Dancer>> CreateDancerAsync(CreateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer != null)
            {
                return Result<Dancer>.Error("Dancer already exists");
            }

            dancer = new Dancer
            {
                Id = Guid.NewGuid(),
                AuthenticationId = requestModel.AuthId,
                DdrCode = requestModel.DdrCode,
                DdrName = requestModel.DdrName,
                State = requestModel.State,
                PrimaryMachineLocation = requestModel.PrimaryMachineLocation
            };

            await _dancerRepository.CreateDancer(dancer, cancellationToken);
            return Result<Dancer>.Success(dancer);
        }

        public async Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel, CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer == null)
            {
                return Result<Dancer>.NotFound();
            }

            dancer.State = requestModel.State;
            dancer.DdrCode = requestModel.DdrCode;
            dancer.DdrName = requestModel.DdrName;
            dancer.PrimaryMachineLocation = requestModel.PrimaryMachineLocation;

            await _dancerRepository.UpdateDancer(dancer, cancellationToken);
            return Result<Dancer>.Success(dancer);
        }

        public Result<IEnumerable<GetDancerBadgesResponseModel>> GetDancerBadges(Guid id)
        {
            var badges = _dancerRepository.GetBadgesForDancer(id);
            return Result<IEnumerable<GetDancerBadgesResponseModel>>.Success(badges);
        }

        public async Task<bool> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
        {
            return await _dancerRepository.AddBadgeToDancer(dancerId, badgeId, cancellationToken);
        }

        public async Task<bool> RemoveBadgeFromDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
        {
            return await _dancerRepository.RemoveBadgeFromDancer(dancerId, badgeId, cancellationToken);
        }

        public async Task<Result<bool>> SetAvatarForDancerByAuthId(string authId, Stream fileStream, CancellationToken cancellationToken)
        {
            var byAuthIdSpec = new ByAuthIdSpec(authId);
            var dancer = await _repository.GetBySpecAsync(byAuthIdSpec, cancellationToken);
            if (dancer == null)
            {
                return Result<bool>.NotFound();
            }
            var imageSizes = new List<int>() {128, 256};
            var baseImage = await Image.LoadAsync(fileStream, cancellationToken);
            
            var uploadProcess = imageSizes.Select(async size =>
            {
                var copiedImage = baseImage.Clone(image => image.Resize(size, size));
                using var stream = new MemoryStream();
                await copiedImage.SaveAsPngAsync(stream, cancellationToken);
                await _fileStorage.UploadFileFromStream(stream, $"profile/avatar/{dancer.Id}.{size}.png");
            });
            
            dancer.ProfilePictureTimestamp = DateTime.UtcNow;
            await _repository.SaveChangesAsync(cancellationToken);

            await Task.WhenAll(uploadProcess);
            return Result<bool>.Success(true);
        }
    }
}