using JobQueueTrigger.Service.Interface;
using Newtonsoft.Json.Linq;

namespace JobQueueTrigger.Service
{
    public class FetchUrlService : IFetchImageService
    {
        public async Task<string> FetchUrl()
        {
            string url = "";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(
                        Environment.GetEnvironmentVariable("unsplashURL") +
                        Environment.GetEnvironmentVariable("client_id") +
                        Environment.GetEnvironmentVariable("urlParams"));

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        return url = getTargetUrl(jsonContent);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (HttpRequestException e)
                {
                    throw e;
                }
            }
        }

        private string getTargetUrl(string jsonContent)
        {
            JArray obj = JArray.Parse(jsonContent);
            return getUrlsObject(obj);
        }

        private string getUrlsObject(JArray arr)
        {
            if (arr[0]["urls"]["small"] == null)
            {
                Console.WriteLine("Small url not found");
            }
            return arr[0]["urls"]["small"].ToString();
        }
    }
}
