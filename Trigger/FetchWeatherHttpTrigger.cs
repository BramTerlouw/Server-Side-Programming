using System.Net;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model.Entity;
using ServerSideProgramming.Model.Enumeration;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Trigger
{
    public class FetchWeatherHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly ITableService _tableService;
        private readonly IBlobService _blobService;

        public FetchWeatherHttpTrigger(
            ILoggerFactory loggerFactory, 
            IBlobService blobService,
            ITableService tableService)
        {
            _logger         = loggerFactory.CreateLogger<FetchWeatherHttpTrigger>();
            _blobService    = blobService;
            _tableService   = tableService;
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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _tableService.InitTable("jobstatus");


            if (req.Query == null || req.Query["jobId"] == null)
            {
                HttpResponseData errorResponse = CreateResponse(req, "No or incorrect query parameter 'jobId' provided!", HttpStatusCode.BadRequest);
                return errorResponse;
            }
            string jobId = $"{req.Query["jobId"]}";


            JobStatus jobStatus = await _tableService.RetrieveRecord(jobId.Split('-')[0], jobId.Replace("-", ""));
            if (jobStatus.Status == (int)StatusType.Pending)
            {
                HttpResponseData infoResponse = CreateResponse(req, "Job is waiting to be processed!", HttpStatusCode.OK);
                return infoResponse;
            }


            if (jobStatus.Status == (int)StatusType.Processing)
            {
                HttpResponseData infoResponse = CreateResponse(req, "Job is still being processed!", HttpStatusCode.OK);
                return infoResponse;
            }


            await _blobService.InitBlobAsync(jobId);
            List<string> urls = await _blobService.GetBlobs();


            string serializedUrls = JsonConvert.SerializeObject(urls);
            HttpResponseData response = CreateResponse(req, serializedUrls, HttpStatusCode.OK);
            return response;
        }

        private HttpResponseData CreateResponse(HttpRequestData req, string message, HttpStatusCode code)
        {
            var response = req.CreateResponse(code);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(message);
            return response;
        }
    }
}
