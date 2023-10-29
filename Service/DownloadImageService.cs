using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Service
{
    public class DownloadImageService : IDownloadImageService
    {
        public async Task<byte[]> GetImage()
        {
            using var httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://picsum.photos/500");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something went wrong during the process of getting the image!");
                }

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
