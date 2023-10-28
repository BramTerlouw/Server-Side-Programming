using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace JobQueueTrigger.Service.Interface
{
    public interface IBlobService
    {
        Task InitBlobAsync(string jobId);
        Task<List<string>> GetBlobs();
        Task CreateBlob(string blobName, byte[] blob);

        Task<UserDelegationKey> RequestUserDelegationKey();
        Uri CreateUserDelegationSASBlob(UserDelegationKey userDelegationKey,BlobItem blobItem);
    }
}
