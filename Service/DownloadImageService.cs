using ServerSideProgramming.Service.Interface;
using System.Drawing;
using System.Net;

namespace ServerSideProgramming.Service
{
    public class DownloadImageService : IDownloadImageService
    {
        public Bitmap getImageFromUrl(string url)
        {
            byte[] dataArr;
            using (WebClient webClient = new WebClient())
            {
                dataArr = webClient.DownloadData(url);
            }
            return convertToBitmap(dataArr);
        }

        private static Bitmap convertToBitmap(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                using (Bitmap bitmap = new Bitmap(stream))
                {
                    return bitmap;
                }
            }
        }
    }
}
