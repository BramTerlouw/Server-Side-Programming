using System.Drawing;

namespace ServerSideProgramming.Service.Interface
{
    public interface IDownloadImageService
    {
        byte[] getImageFromUrl(string url);
    }
}
