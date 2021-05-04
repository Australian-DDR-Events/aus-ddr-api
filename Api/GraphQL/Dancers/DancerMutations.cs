using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.Helpers;
using AusDdrApi.Persistence;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.FileStorage;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using SixLabors.ImageSharp;

namespace AusDdrApi.GraphQL.Dancers
{
    [ExtendObjectType("Mutation")]
    public class DancerMutations
    {
        [UseDatabaseContext]
        [Authorize]
        public async Task<AddDancerPayload> AddDancerAsync(
            AddDancerInput input,
            [ScopedService] DatabaseContext context,
            [Service] IFileStorage fileStorage,
            [Service] IAuthorization authorization,
            CancellationToken cancellationToken)
        {
            var authId = authorization.GetUserId();
            if (authId == null)
            {
                return new AddDancerPayload(
                new []
                {
                    new UserError("Cannot find auth id.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                });
            }
            
            if (context.Dancers.Any(d => d.AuthenticationId == authId))
            {
                return new AddDancerPayload(
                    new []
                    {
                        new UserError("Auth id already has associated dancer.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                    });
            }

            var dancer = new Dancer
            {
                DdrCode = input.DdrCode,
                DdrName = input.DdrName,
                State = input.State,
                PrimaryMachineLocation = input.PrimaryMachineLocation,
                AuthenticationId = authId
            };
            
            if (input.ProfilePicture != null)
            {
                dancer.ProfilePictureTimestamp = DateTime.UtcNow;
                try
                {
                    using var profileImage = await Image.LoadAsync(input.ProfilePicture.OpenReadStream());
                    var image = await Images.ImageToPngMemoryStreamFactor(profileImage, 256, 256);
                
                    var destinationKey = $"profile/picture/{dancer.Id}.{(dancer.ProfilePictureTimestamp?.Ticks - 621355968000000000) / 10000000}.png";
                    await fileStorage.UploadFileFromStream(image, destinationKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new AddDancerPayload(
                        new []
                        {
                            new UserError("Failed to upload profile picture.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
                        });
                }
            }

            await context.Dancers.AddAsync(dancer, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new AddDancerPayload(dancer);
        }

        [UseDatabaseContext]
        [Authorize]
        public async Task<UpdateDancerPayload> UpdateDancerAsync(
            UpdateDancerInput input,
            [ScopedService] DatabaseContext context,
            [Service]IAuthorization authorization,
            [Service] IFileStorage fileStorage,
            CancellationToken cancellationToken)
        {
            var authId = authorization.GetUserId();
            var dancer = await context.Dancers.FindAsync(new object[]{input.DancerId}, cancellationToken);

            if (dancer.AuthenticationId != authId)
            {
                return new UpdateDancerPayload(
                    new []
                    {
                        new UserError("Cannot update unmatched subject.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                    });
            }

            if (dancer is null)
            {
                return new UpdateDancerPayload(
                    new []
                    {
                        new UserError("Dancer not found.", DancerErrorCodes.DANCER_NOT_FOUND)
                    });
            }

            dancer.DdrCode = input.DdrCode;
            dancer.DdrName = input.DdrName;
            dancer.State = input.State;
            dancer.PrimaryMachineLocation = input.PrimaryMachineLocation;

            if (input.ProfilePicture != null)
            {
                dancer.ProfilePictureTimestamp = DateTime.UtcNow;
                try
                {
                    using var profileImage = await Image.LoadAsync(input.ProfilePicture.OpenReadStream());
                    var image = await Images.ImageToPngMemoryStreamFactor(profileImage, 256, 256);
                
                    var destinationKey = $"profile/picture/{dancer.Id}.{(dancer.ProfilePictureTimestamp?.Ticks - 621355968000000000) / 10000000}.png";
                    await fileStorage.UploadFileFromStream(image, destinationKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new UpdateDancerPayload(
                    new []
                    {
                        new UserError("Failed to upload profile picture.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
                    });
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateDancerPayload(dancer);
        }
    }
}