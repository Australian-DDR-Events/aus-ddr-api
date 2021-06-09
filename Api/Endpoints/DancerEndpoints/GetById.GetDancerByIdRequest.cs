using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public class GetDancerByIdRequest
    {
        public const string Route = "/dancers/{Id:guid}";
        public static string BuildRoute(Guid id) => Route.Replace("{Id:guid}", id.ToString());
        public Guid Id { get; set; }
    }
}