using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobQueueTrigger.Service.Interface;
using System.Text;

namespace JobQueueTrigger.Service
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _containerClient;
        private BlobClient _blobClient;

        public BlobService()
        {
            _blobServiceClient  = new BlobServiceClient("UseDevelopmentStorage=true");
            _containerClient    = _blobServiceClient.GetBlobContainerClient("images");
        }

        public async Task InitBlobAsync(string blob)
        {
            _blobClient = _containerClient.GetBlobClient(blob);
        }

        public async Task CreateBlob(string blob)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(blob)))
            {
                await _blobClient.UploadAsync(stream, true);
            }
        }

        public async Task GetBlob()
        {
            BlobDownloadInfo blobDownloadInfo = await _blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(blobDownloadInfo.Content))
            {
                string content = await streamReader.ReadToEndAsync();
            }
        }
    }
}
