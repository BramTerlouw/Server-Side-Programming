namespace JobQueueTrigger.Service.Interface
{
    public interface IFetchImageService
    {
        Task<string> FetchImageAsync();
    }
}
