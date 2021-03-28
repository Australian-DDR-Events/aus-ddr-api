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
            
            // descriptor
            //     .Field(d => d.Badges)
            //     .ResolveWith<DancerResolvers>(d => d.GetBadgesAsync(default!, default!, default!, default))
            //     .UseDbContext<DatabaseContext>()
            //     .Name("badges");
        }

        // private class DancerResolvers
        // {
        //     public async Task<IEnumerable<Badge>> GetBadgesAsync(
        //         Dancer dancer,
        //         [ScopedService] DatabaseContext dbContext,
        //         SessionByIdDataLoader sessionById,
        //         CancellationToken cancellationToken)
        //     {
        //         int[] speakerIds = await dbContext.Dancers
        //             .Include(d => d.Badges)
        //             .FirstAsync(d => d.Id == dancer.Id)
        //             .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
        //             .ToArrayAsync();        
        //
        //         return await sessionById.LoadAsync(speakerIds, cancellationToken);
        //     }
        // }
    }
}