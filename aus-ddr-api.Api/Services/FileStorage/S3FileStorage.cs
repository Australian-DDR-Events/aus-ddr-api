using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using AusDdrApi.Context;

namespace AusDdrApi.Services.FileStorage
{
    public class S3FileStorage : IFileStorage
    {
        private readonly IAmazonS3 _s3Client;

        private readonly AwsConfiguration _awsConfiguration;
        
        public S3FileStorage(IAmazonS3 s3Client, AwsConfiguration awsConfiguration)
        {
            _s3Client = s3Client;
            _awsConfiguration = awsConfiguration;
        }
        
        public async Task<string> UploadFileFromStream(Stream stream, string destination)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = destination,
                BucketName = _awsConfiguration.AssetsBucketName,
                CannedACL = S3CannedACL.PublicRead,
            };

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            await stream.DisposeAsync();

            return
                $"https://{_awsConfiguration.AssetsBucketName}.s3-{_awsConfiguration.AssetsBucketLocation}.amazonaws.com/{destination}";
        }
    }
}