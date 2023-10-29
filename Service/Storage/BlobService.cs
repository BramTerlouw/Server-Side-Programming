using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobQueueTrigger.Service.Interface;

namespace ServerSideProgramming.Service.Storage
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _containerClient;
        private BlobClient _blobClient;

        public BlobService()
        {
            _blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        }

        public async Task InitBlobAsync(string jobId)
        {
            _containerClient = _blobServiceClient.GetBlobContainerClient(jobId);
            await _containerClient.CreateIfNotExistsAsync();
            _containerClient.SetAccessPolicy(PublicAccessType.Blob);
        }

        public async Task CreateBlob(string blobName, byte[] blob)
        {
            _blobClient = _containerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(blob))
            {
                await _blobClient.UploadAsync(stream, true);
            }
        }

        public async Task<List<string>> GetBlobs()
        {
            List<string> urls = new List<string>();

            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                urls.Add(_containerClient.GetBlobClient(blobItem.Name).Uri.ToString());
            }
            return urls;
        }

        public async Task<UserDelegationKey> RequestUserDelegationKey()
        {
            UserDelegationKey userDelegationKey =
                await _blobServiceClient.GetUserDelegationKeyAsync(
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow.AddDays(1));

            return userDelegationKey;
        }
    }
}
