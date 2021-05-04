using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Dancers
{
    public record AddDancerInput(
        string DdrName, 
        string DdrCode, 
        string PrimaryMachineLocation, 
        string State,
        IFile? ProfilePicture);
}