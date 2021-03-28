using AusDdrApi.Entities;
using AusDdrApi.GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Types
{
    public class DancerType : ObjectType<Dancer>
    {
        protected override void Configure(IObjectTypeDescriptor<Dancer> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(dancer => dancer.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<DancerByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
        }
    }
}