using System;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AusDdrApi.Extensions
{
    public abstract class EndpointWithResponse<TRequest, TResponse, TEntity> : BaseAsyncEndpoint.WithRequest<TRequest>.WithResponse<TResponse>
    {
        [NonAction]
        public abstract TResponse Convert(TEntity u);
    }

    public abstract class EndpointWithoutResponse<T> : BaseAsyncEndpoint.WithRequest<T>.WithoutResponse
    {
    }
}