using ServerSideProgramming.Service.Interface;
using System.Drawing;
using System.Net;

namespace ServerSideProgramming.Service
{
    public class DownloadImageService : IDownloadImageService
    {
        public byte[] getImageFromUrl(string url)
        {
            byte[] dataArr;
            using (WebClient webClient = new WebClient())
            {
                return dataArr = webClient.DownloadData(url);
            }
        }
    }
}
