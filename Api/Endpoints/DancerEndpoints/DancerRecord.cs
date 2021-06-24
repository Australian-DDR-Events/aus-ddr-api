using System;

namespace AusDdrApi.Endpoints.DancerEndpoints
{
    public record DancerRecord(Guid id, string name, string code, string primaryMachine, string state, string profilePictureUrl);
}