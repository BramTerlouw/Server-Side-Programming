using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
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
            _containerClient.SetAccessPolicy(PublicAccessType.None);
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
            string sasToken = getBlobContainerSasToken();

            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                string fileUri = _containerClient.GetBlobClient(blobItem.Name).Uri.ToString();
                urls.Add(combineUriWithSASToken(fileUri, sasToken));
            }
            return urls;
        }

        private string combineUriWithSASToken(string fileUri,  string sasToken)
        {
            return $"{fileUri}?{sasToken}";
        }

        private string getBlobContainerSasToken()
        {
            Uri uri = getBlobContainerSasUri();
            return uri.ToString().Split('?')[1];
        }

        private Uri getBlobContainerSasUri()
        {
            BlobSasBuilder builder = new BlobSasBuilder();
            builder.BlobContainerName = _containerClient.Name;
            builder.SetPermissions(BlobAccountSasPermissions.Read | BlobAccountSasPermissions.List);
            builder.StartsOn = DateTimeOffset.Now.AddDays(-1);
            builder.ExpiresOn = DateTimeOffset.Now.AddDays(1);
            return _containerClient.GenerateSasUri(builder);
        }
    }
}
