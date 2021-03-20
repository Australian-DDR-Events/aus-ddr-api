using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Dancers
{
    [ExtendObjectType("Mutation")]
    public class DancerMutations
    {
        [UseDatabaseContext]
        public async Task<AddDancerPayload> AddDancerAsync(
            AddDancerInput input,
            [ScopedService] DatabaseContext context,
            CancellationToken cancellationToken)
        {
            var dancer = new Dancer
            {
                DdrCode = input.DdrCode,
                DdrName = input.DdrName,
                State = input.State,
                PrimaryMachineLocation = input.PrimaryMachineLocation,
                AuthenticationId = input.AuthenticationId
            };

            context.Dancers.Add(dancer);
            await context.SaveChangesAsync(cancellationToken);

            return new AddDancerPayload(dancer);
        }

        [UseDatabaseContext]
        public async Task<UpdateDancerPayload> UpdateDancerAsync(
            UpdateDancerInput input,
            [ScopedService] DatabaseContext context)
        {
            var dancer = await context.Dancers.FindAsync(input.DancerId);

            if (dancer is null)
            {
                return new UpdateDancerPayload(
                    new []
                    {
                        new UserError("Dancer not found.", "DANCER_NOT_FOUND")
                    });
            }

            dancer.DdrCode = input.DdrCode;
            dancer.DdrName = input.DdrName;
            dancer.State = input.State;
            dancer.PrimaryMachineLocation = input.PrimaryMachineLocation;

            await context.SaveChangesAsync();

            return new UpdateDancerPayload(dancer);
        }
    }
}