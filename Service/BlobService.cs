using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobQueueTrigger.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace JobQueueTrigger.Service
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient containerClient;
        private readonly BlobClient blobClient;

        public BlobService()
        {
            blobServiceClient = new BlobServiceClient("");
            containerClient = blobServiceClient.GetBlobContainerClient("");
            blobClient = containerClient.GetBlobClient("");

        }
        public async Task CreateBlob(string blob)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(blob)))
            {
                await blobClient.UploadAsync(stream, true);
                Console.WriteLine("Blob created with new content.");
            }
        }

        public async Task GetBlobs()
        {
            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(blobDownloadInfo.Content))
            {
                string content = await streamReader.ReadToEndAsync();
                Console.WriteLine($"Blob Content: {content}");
            }
        }
    }
}
