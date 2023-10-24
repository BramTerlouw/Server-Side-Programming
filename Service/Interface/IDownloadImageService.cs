namespace ServerSideProgramming.Service.Interface
{
    public interface IDownloadImageService
    {
        Task<byte[]> GetImage();
    }
}
