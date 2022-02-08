namespace Application.Core.Models.Dancer
{
    public class CreateDancerRequestModel
    {
        public string AuthId { get; set; }
        public string DdrName { get; set; } = string.Empty;
        public string DdrCode { get; set; } = string.Empty;
        public string PrimaryMachineLocation { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        
    }
}