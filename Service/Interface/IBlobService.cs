namespace JobQueueTrigger.Service.Interface
{
    public interface IBlobService
    {
        Task GetBlobs();
        Task CreateBlob(string blob);
    }
}
