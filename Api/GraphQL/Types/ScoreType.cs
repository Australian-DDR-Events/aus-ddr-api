using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Types
{
    public class ScoreType : ObjectType<Score>
    {
        protected override void Configure(IObjectTypeDescriptor<Score> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(score => score.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<ScoreByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.SongDifficulty)
                .ResolveWith<ScoreResolvers>(t => t.GetSongDifficultyAsync(default!, default!, default));
            descriptor
                .Field(t => t.SongDifficultyId)
                .ID(nameof(SongDifficulty));

            descriptor
                .Field(t => t.Dancer)
                .ResolveWith<ScoreResolvers>(t => t.GetDancerAsync(default!, default!, default));
            descriptor
                .Field(t => t.DancerId)
                .ID(nameof(Dancer));
        }

        private class ScoreResolvers
        {
            public Task<Dancer> GetDancerAsync(
                Score score,
                DancerByIdDataLoader dancerById,
                CancellationToken cancellationToken)
            {
                return score == null ? null : dancerById.LoadAsync(score.DancerId, cancellationToken);
            }
            
            public Task<SongDifficulty> GetSongDifficultyAsync(
                Score score,
                SongDifficultyByIdDataLoader songDifficultyById,
                CancellationToken cancellationToken)
            {
                return score == null ? null : songDifficultyById.LoadAsync(score.SongDifficultyId, cancellationToken);
            }
        }
    }
}