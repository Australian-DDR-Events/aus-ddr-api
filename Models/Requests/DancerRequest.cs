namespace AusDdrApi.Models.Requests
{
    public class DancerRequest
    {
        public string AuthenticationId { get; set; }
        public string DdrName { get; set; }
        public string DdrCode { get; set; }
        public string PrimaryMachineLocation { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }
        
        public Dancer ToDancer() => new Dancer
        {
            AuthenticationId = AuthenticationId,
            DdrName = DdrName,
            DdrCode = DdrCode,
            PrimaryMachineLocation = PrimaryMachineLocation,
            State = State,
            ProfilePictureUrl = ProfilePictureUrl
        };
    }
}