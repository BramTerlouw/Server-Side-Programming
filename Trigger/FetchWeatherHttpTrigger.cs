using System.Net;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServerSideProgramming.Trigger
{
    public class FetchWeatherHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IBlobService _blobService;

        public FetchWeatherHttpTrigger(ILoggerFactory loggerFactory, IBlobService blobService)
        {
            _logger = loggerFactory.CreateLogger<FetchWeatherHttpTrigger>();
            _blobService = blobService;
        }


        /// <summary>
        /// Method Run is a Azure HTTP trigger which functions like an endpoint. This method lists if
        /// the job is busy or all generated images with weather data.
        /// </summary>
        /// 
        /// <param name="req">
        /// Req is a HTTP GET request.
        /// </param>
        /// 
        /// <returns>
        /// ...
        /// </returns>
        [Function("FetchWeatherHttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await _blobService.InitBlobAsync("test");
            List<string> urls = await _blobService.GetBlobs();

            // Get status of job or list of images with weather data
            // ...


            // Use job id provided by other http trigger endpoint
            // ...


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            string serializedUrls = JsonConvert.SerializeObject(urls);
            response.WriteString(serializedUrls);

            return response;
        }
    }
}
