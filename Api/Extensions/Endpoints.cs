using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Extensions
{
    public abstract class EndpointWithResponse<TRequest, TResponse, TEntity> : BaseAsyncEndpoint.WithRequest<TRequest>.WithResponse<TResponse>
    {
        [NonAction]
        public abstract TResponse Convert(TEntity u);
    }
    
    public abstract class EndpointWithResponse<TResponse, TEntity> : BaseAsyncEndpoint.WithoutRequest.WithResponse<TResponse>
    {
        [NonAction]
        public abstract TResponse Convert(TEntity u);
    }

    public abstract class EndpointWithoutResponse<T> : BaseAsyncEndpoint.WithRequest<T>.WithoutResponse
    {
    }

    public abstract class EndpointWithoutResponse : BaseAsyncEndpoint.WithoutRequest.WithoutResponse
    {
    }
}