namespace JobQueueTrigger.Service.Interface
{
    public interface IBlobService
    {
        Task InitBlobAsync(string jobId);
        Task<List<string>> GetBlobs();
        Task CreateBlob(string blobName, byte[] blob);
    }
}
