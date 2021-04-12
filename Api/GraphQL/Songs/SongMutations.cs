using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.GraphQL.Dancers;
using AusDdrApi.Helpers;
using AusDdrApi.Persistence;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.FileStorage;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using SixLabors.ImageSharp;

namespace AusDdrApi.GraphQL.Songs
{
    [ExtendObjectType("Mutation")]
    public class SongMutations
    {
        [UseDatabaseContext]
        [Authorize(Policy = "Admin")]
        public async Task<AddSongPayload> AddSongAsync(
            AddSongInput input,
            [ScopedService] DatabaseContext context,
            [Service] IAuthorization authorization,
            [Service] IFileStorage fileStorage,
            CancellationToken cancellationToken)
        {
            var authId = authorization.GetUserId();

            var song = new Song
            {
                Name = input.Name,
                Artist = input.Artist,
                Difficulty = input.Difficulty,
                Level = input.Level,
                MaxScore = input.MaxScore
            };

            var songEntity = await context.Songs.AddAsync(song, cancellationToken);
            
            if (input.SongJacket != null)
            {
                var uploadTasks = new List<Task<string>>();
                try
                {
                    int[] imageSizes = {32, 64, 128, 256, 512};
                    using var ingredientImage = await Image.LoadAsync(input.SongJacket!.OpenReadStream());
                    foreach (var size in imageSizes)
                    {
                        var image = await Images.ImageToPngMemoryStream(ingredientImage, size, size);

                        var destinationKey = $"songs/{songEntity.Entity.Id}.{size}.png";
                        uploadTasks.Add(fileStorage.UploadFileFromStream(image, destinationKey));
                    }
                }
                catch
                {
                    return new AddSongPayload(
                        new []
                        {
                            new UserError("Failed to upload song jacket.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
                        }
                    );
                }
                
                var t = Task.WhenAll(uploadTasks);
                try
                {
                    t.Wait(cancellationToken);
                }
                catch
                {
                    return new AddSongPayload(
                        new []
                        {
                            new UserError("Failed to upload song jacket.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
                        }
                    );
                }
            }
            
            await context.SaveChangesAsync(cancellationToken);

            return new AddSongPayload(songEntity.Entity);
        }

        [UseDatabaseContext]
        [Authorize]
        public async Task<UpdateDancerPayload> UpdateDancerAsync(
            UpdateDancerInput input,
            [ScopedService] DatabaseContext context,
            [Service]IAuthorization authorization,
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

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateDancerPayload(dancer);
        }
    }
}