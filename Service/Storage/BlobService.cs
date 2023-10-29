using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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
            // SAS TOKEN STILL IN PROGRESS
            //string accountName = "...";
            //string accountKey = "...";
            //StorageSharedKeyCredential storageSharedKeyCredential = new(accountName, accountKey);
            //_blobServiceClient = new BlobServiceClient(new Uri("..."), storageSharedKeyCredential);

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
            //UserDelegationKey sasToken = await RequestUserDelegationKey();
            List<string> urls = new List<string>();

            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                // SAS TOKEN STILL IN PROGRESS
                //Uri uri = CreateUserDelegationSASBlob(sasToken, blobItem);
                //urls.Add(uri.ToString());

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

        public Uri CreateUserDelegationSASBlob(
            UserDelegationKey userDelegationKey,
            BlobItem blobItem)
        {
            Uri blobUri = _containerClient.GetBlobClient(blobItem.Name).Uri;

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobItem.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            BlobUriBuilder uriBuilder = new BlobUriBuilder(blobUri)
            {
                // Specify the user delegation key
                Sas = sasBuilder.ToSasQueryParameters(
                    userDelegationKey,
                    _containerClient.GetParentBlobServiceClient().AccountName)
            };

            return uriBuilder.ToUri();
        }
    }
}
