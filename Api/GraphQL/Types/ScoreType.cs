using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
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
        }

        private class ScoreResolvers
        {
        }
    }
}