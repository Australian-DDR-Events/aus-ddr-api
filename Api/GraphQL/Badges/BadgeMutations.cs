using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.Helpers;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using SixLabors.ImageSharp;

namespace AusDdrApi.GraphQL.Badges
{
    [ExtendObjectType("Mutation")]
    public class BadgeMutations
    {
        [UseDatabaseContext]
        [Authorize(Policy = "Admin")]
        public async Task<AddBadgePayload> AddBadgeAsync(
            AddBadgeInput input,
            [ScopedService] DatabaseContext context,
            [Service] IFileStorage fileStorage,
            CancellationToken cancellationToken)
        {
            if (input.BadgeImage == null)
            {
                return new AddBadgePayload(
                    new []
                    {
                        new UserError("No badge image provided.", BadgeErrorCodes.BADGE_IMAGE_NOT_PROVIDED)
                    }
                );
            }
            var badge = new Badge
            {
                Name = input.Name,
                Description = input.Description,
                EventId = input.EventId
            };

            var badgeEntity = await context.Badges.AddAsync(badge, cancellationToken);
            
            var uploadTasks = new List<Task<string>>();
            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                using var ingredientImage = await Image.LoadAsync(input.BadgeImage!.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(ingredientImage, size, size);

                    var destinationKey = $"badges/{badgeEntity.Entity.Id}.{size}.png";
                    uploadTasks.Add(fileStorage.UploadFileFromStream(image, destinationKey));
                }
            }
            catch
            {
                return new AddBadgePayload(
                    new []
                    {
                        new UserError("Failed to upload badge image.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
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
                return new AddBadgePayload(
                    new []
                    {
                        new UserError("Failed to upload badge image.", CommonErrorCodes.IMAGE_UPLOAD_FAILED)
                    }
                );
            }
            
            await context.SaveChangesAsync(cancellationToken);

            return new AddBadgePayload(badgeEntity.Entity);
        }

        [UseDatabaseContext]
        [Authorize(Policy = "Admin")]
        public async Task<AddBadgeAllocationPayload> AddBadgeAllocationAsync(
            AddBadgeAllocationInput input,
            [ScopedService] DatabaseContext context,
            CancellationToken cancellationToken)
        {
            var dancer = await context.Dancers.FindAsync(new object[]{
                input.DancerId
            }, cancellationToken);
            if (dancer == null) return new AddBadgeAllocationPayload(
                new []
                {
                    new UserError("Dancer does not exist.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                }
            );
            var badge = await context.Badges.FindAsync(new object[]
            {
                input.BadgeId
            }, cancellationToken);
            if (dancer == null) return new AddBadgeAllocationPayload(
                new []
                {
                    new UserError("Badge does not exist.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                }
            );
            dancer.Badges.Add(badge);
            await context.SaveChangesAsync(cancellationToken);

            return new AddBadgeAllocationPayload();
        }
    }
}