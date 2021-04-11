using System;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Summer2021
{
    public record GradedIngredientInput(
        int Score,
        Guid IngredientId,
        IFile ScoreEvidenceImage
    );
}