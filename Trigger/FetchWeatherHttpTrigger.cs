using System.Net;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Trigger
{
    public class FetchWeatherHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IQueueService _queueService;
        private readonly IBlobService _blobService;

        public FetchWeatherHttpTrigger(
            ILoggerFactory loggerFactory, 
            IBlobService blobService,
            IQueueService queueService)
        {
            _logger         = loggerFactory.CreateLogger<FetchWeatherHttpTrigger>();
            _queueService   = queueService;
            _blobService    = blobService;
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
        /// Returns a HttpResponseData object which contains either an error message, an informational message
        /// telling the user that the process is still running, or a list with all the images which can be
        /// seen in the browser.
        /// </returns>
        [Function("FetchWeatherHttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            if (req.Query == null || req.Query["jobId"] == null)
            {
                HttpResponseData errorResponse = CreateResponse(req, "No or incorrect query parameter 'jobId' provided!");
                return errorResponse;
            }
            string jobId = req.Query["jobId"]?.ToString();

            _queueService.InitQueue("writes");
            if (await _queueService.MessagesStillInQueue(jobId))
            {
                HttpResponseData infoResponse = CreateResponse(req, "Images are still being processed!");
                return infoResponse;
            }
            

            await _blobService.InitBlobAsync(jobId);
            List<string> urls = await _blobService.GetBlobs();

            string serializedUrls = JsonConvert.SerializeObject(urls);
            HttpResponseData response = CreateResponse(req, serializedUrls);
            return response;
        }

        private HttpResponseData CreateResponse(HttpRequestData req, string message)
        {
            var response = req.CreateResponse(HttpStatusCode.BadGateway);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(message);
            return response;
        }
    }
}
