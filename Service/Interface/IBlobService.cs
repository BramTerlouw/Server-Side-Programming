namespace JobQueueTrigger.Service.Interface
{
    public interface IBlobService
    {
        Task InitBlobAsync(string blob);
        Task GetBlob();
        Task CreateBlob(string blob);
    }
}
