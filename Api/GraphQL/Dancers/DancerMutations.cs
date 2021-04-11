using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.Persistence;
using AusDdrApi.Services.Authorization;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

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
            [Service] IAuthorization authorization,
            CancellationToken cancellationToken)
        {
            var authId = authorization.GetUserId();

            var dancer = new Dancer
            {
                DdrCode = input.DdrCode,
                DdrName = input.DdrName,
                State = input.State,
                PrimaryMachineLocation = input.PrimaryMachineLocation,
                AuthenticationId = authId
            };

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