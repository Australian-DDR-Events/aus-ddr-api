using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL
{
    [ExtendObjectType("Query")]
    public class DancerQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Dancer> GetDancers([ScopedService] DatabaseContext context) => context.Dancers;

        public Task<Dancer?> GetDancerByIdAsync(
            [ID(nameof(Dancer))] Guid id,
            DancerByIdDataLoader dancerByIdDataLoader,
            CancellationToken cancellationToken) => dancerByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<Dancer>> GetDancersByIdAsync(
            [ID(nameof(Dancer))] Guid[] ids,
            DancerByIdDataLoader dancerByIdDataLoader,
            CancellationToken cancellationToken) => await dancerByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<Dancer>();
    }
}