namespace AusDdrApi.Context
{
    public class AwsConfiguration
    {
        public string AssetsBucketName { get; set; }
        public string AssetsBucketLocation { get; set; }

        public AwsConfiguration()
        {
            AssetsBucketName = "";
        }
    }
}