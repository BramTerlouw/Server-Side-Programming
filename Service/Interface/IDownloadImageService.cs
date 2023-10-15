using System.Drawing;

namespace ServerSideProgramming.Service.Interface
{
    public interface IDownloadImageService
    {
        Bitmap getImageFromUrl(string url);
    }
}
