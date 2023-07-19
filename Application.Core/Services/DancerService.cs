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
using Application.Core.Models;
using Application.Core.Models.Dancer;
using Microsoft.CodeAnalysis;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Core.Services
{
    public class DancerService : IDancerService
    {
        private readonly IDancerRepository _dancerRepository;
        private readonly IFileStorage _fileStorage;

        public DancerService(IDancerRepository dancerRepository, IFileStorage fileStorage)
        {
            _dancerRepository = dancerRepository;
            _fileStorage = fileStorage;
        }

        public async Task<Result<IEnumerable<Dancer>>> GetDancersAsync(int page, int limit, CancellationToken cancellationToken)
        {
            var skip = page * limit;
            var dancers = _dancerRepository.GetDancers(skip, limit);
            return new Result<IEnumerable<Dancer>>
            {
                ResultCode = ResultCode.Ok,
                Value = new Optional<IEnumerable<Dancer>>(dancers)
            };
        }

        public Result<Dancer> GetDancerById(Guid id)
        {
            var dancer = _dancerRepository.GetDancerById(id);
            return dancer != null
                ? new Result<Dancer>
                {
                    ResultCode = ResultCode.Ok,
                    Value = dancer,
                }
                : new Result<Dancer>
                {
                    ResultCode = ResultCode.NotFound, 
                    Value = new Optional<Dancer>(),
                };
        }

        public async Task<Result<Dancer>> MigrateDancer(MigrateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer != null) return new Result<Dancer>
            {
                ResultCode = ResultCode.Ok, 
                Value = dancer
                
            };
            if (requestModel.LegacyId == null) return new Result<Dancer>
                {
                    ResultCode = ResultCode.NotFound, 
                    Value = new Optional<Dancer>(),
                };
            
            dancer = _dancerRepository.GetDancerByAuthId(requestModel.LegacyId);
            if (dancer == null) return new Result<Dancer>
            {
                ResultCode = ResultCode.NotFound,
                Value = new Optional<Dancer>(),
                
            };

            dancer.AuthenticationId = requestModel.AuthId;
            await _dancerRepository.UpdateDancer(dancer, cancellationToken);

            return new Result<Dancer>
            {
                ResultCode = ResultCode.Ok, 
                Value = dancer
                
            };
        }

        public async Task<Result<Dancer>> CreateDancerAsync(CreateDancerRequestModel requestModel,
            CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer != null)
            {
                return new Result<Dancer>
                {
                    ResultCode = ResultCode.Conflict,
                    Value = dancer
                };
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
            return new Result<Dancer>
            {
                ResultCode = ResultCode.Ok,
                Value = dancer
            };
        }

        public async Task<Result<Dancer>> UpdateDancerAsync(UpdateDancerRequestModel requestModel, CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(requestModel.AuthId);
            if (dancer == null)
            {
                return new Result<Dancer>
                {
                    ResultCode = ResultCode.NotFound,
                    Value = new Optional<Dancer>(),
                };
            }

            dancer.State = requestModel.State;
            dancer.DdrCode = requestModel.DdrCode;
            dancer.DdrName = requestModel.DdrName;
            dancer.PrimaryMachineLocation = requestModel.PrimaryMachineLocation;

            await _dancerRepository.UpdateDancer(dancer, cancellationToken);
            return new Result<Dancer>
            {
                ResultCode = ResultCode.Ok,
                Value = dancer,
            };
        }

        public Result<IEnumerable<GetDancerBadgesResponseModel>> GetDancerBadges(Guid id)
        {
            var badges = _dancerRepository.GetBadgesForDancer(id);
            return new Result<IEnumerable<GetDancerBadgesResponseModel>>
            {
                ResultCode = ResultCode.Ok,
                Value = new Optional<IEnumerable<GetDancerBadgesResponseModel>>(badges)
            };
        }

        public async Task<Result> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
        {
            await _dancerRepository.AddBadgeToDancer(dancerId, badgeId, cancellationToken);
            return new Result
            {
                ResultCode = ResultCode.Ok,
            };
        }

        public async Task<Result> RemoveBadgeFromDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
        {
            await _dancerRepository.RemoveBadgeFromDancer(dancerId, badgeId, cancellationToken);
            return new Result
            {
                ResultCode = ResultCode.Ok,
            };
        }

        public async Task<Result> SetAvatarForDancerByAuthId(string authId, Stream fileStream, CancellationToken cancellationToken)
        {
            var dancer = _dancerRepository.GetDancerByAuthId(authId);
            if (dancer == null)
            {
                return new Result
                {
                    ResultCode = ResultCode.BadRequest,
                };
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
            await _dancerRepository.UpdateDancer(dancer, cancellationToken);

            await Task.WhenAll(uploadProcess);
            return new Result
            {
                ResultCode = ResultCode.Ok,
            };
        }
    }
}