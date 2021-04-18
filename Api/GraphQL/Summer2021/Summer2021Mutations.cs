using System;
using System.Linq;
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
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.GraphQL.Summer2021
{
    [ExtendObjectType("Mutation")]
    public class Summer2021Mutations
    {
        [UseDatabaseContext]
        [Authorize]
        public async Task<GradedIngredientPayload> SubmitScoreForIngredient(
            GradedIngredientInput input,
            [ScopedService] DatabaseContext context,
            [Service] IAuthorization authorization,
            [Service] IFileStorage fileStorage,
            [Service] ILogger<Summer2021Mutations> logger,
                CancellationToken cancellationToken)
        {
            // Can user perform the current operation?
            var authId = authorization.GetUserId();
            if (authId == null)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError("Cannot update unmatched subject.", CommonErrorCodes.ACT_AGAINST_INVALID_SUBJECT)
                    });
            }
            
            var existingDancer = context.Dancers.SingleOrDefault(d => d.AuthenticationId == authId);
            if (existingDancer == null)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError("Dancer not found", DancerErrorCodes.DANCER_NOT_FOUND)
                    });
            }

            var ingredient = await context.Ingredients.FindAsync(input.IngredientId, cancellationToken);
            if (ingredient == null)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError("Ingredient not found", Summer2021ErrorCodes.INGREDIENT_NOT_FOUND)
                    });
            }
            if (ingredient.SongDifficulty == null)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError("Song not found", Summer2021ErrorCodes.SONG_NOT_FOUND)
                    });
            }
            if (input.Score > ingredient.SongDifficulty!.MaxScore)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError("Invalid score - score is higher than max score", Summer2021ErrorCodes.SCORE_HIGHER_THAN_MAX)
                    });
            }

            // Check if the given score and ingredient actually have a matching graded ingredient
            var gradedIngredient = context
                .GradedIngredients
                .AsQueryable()
                .Where(g => g.IngredientId == input.IngredientId && g.RequiredScore <= input.Score)
                .OrderByDescending(i => i.RequiredScore)
                .FirstOrDefault();
            if (gradedIngredient?.Ingredient == null)
            {
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError(
                            $"Cannot find graded ingredient for the score {input.Score} and ingredient {input.IngredientId}", 
                            Summer2021ErrorCodes.GRADED_INGREDIENT_NOT_FOUND)
                    });
            }

            var score = await context.Scores.AddAsync(new Score
            {
                SongDifficultyId = ingredient.SongDifficultyId,
                DancerId = existingDancer.Id,
                Value = input.Score
            }, cancellationToken);

            var dancerIngredient = await context.GradedDancerIngredients.AddAsync(new GradedDancerIngredient
            {
                GradedIngredientId = gradedIngredient.Id,
                DancerId = existingDancer.Id,
                ScoreId = score.Entity.Id
            }, cancellationToken);

            try
            {
                using var scoreImage = await Image.LoadAsync(input.ScoreEvidenceImage!.OpenReadStream());
                var image = await Images.ImageToPngMemoryStreamFactor(scoreImage, 1000, 1000);

                var destinationKey = $"songs/{score.Entity.SongDifficultyId}/scores/{score.Entity.Id}.png";
                await fileStorage.UploadFileFromStream(image, destinationKey);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, $"Failed to add score image to file store: {e.Message}");
                return new GradedIngredientPayload(
                    new []
                    {
                        new UserError(
                            "The image uploaded was invalid or malformed",
                            CommonErrorCodes.INVALID_IMAGE_FILE
                        )
                    });
            }

            await context.SaveChangesAsync(cancellationToken);

            return new GradedIngredientPayload(dancerIngredient.Entity);
        }
    }
}