using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Trigger
{
    public class GetWeatherHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IQueueService _queueService;
        private readonly ITableService _tableService;

        public GetWeatherHttpTrigger(
            ILoggerFactory loggerFactory, 
            IQueueService queueService, 
            ITableService tableService)
        {
            _logger = loggerFactory.CreateLogger<GetWeatherHttpTrigger>();
            _queueService = queueService;
            _tableService = tableService;
        }


        /// <summary>
        /// Method Run is a Azure HTTP trigger which functions like an endpoint. This method initializes
        /// a new job by creating a job id and adding it to the queue.
        /// </summary>
        /// 
        /// <param name="req">
        /// Req is a HTTP GET request. It should contain 'jobName' query param in url.
        /// </param>
        /// 
        /// <returns>
        /// Returns CustomOutputType obj as response, containing the job ID and HTTP response.
        /// </returns>
        [Function("GetWeatherHttpTrigger")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _queueService.InitQueue("jobs");
            _tableService.InitTable("jobstatus");

            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            if (req.Query == null || req.Query["jobName"] == null)
            {
                return CreateResponse(
                    req,
                    "No or incorrect query parameter 'jobName' provided!", 
                    HttpStatusCode.BadRequest);
            }

            string jobId = $"{timestamp}-{req.Query["jobName"]?.ToString()}";
            await _queueService.SendMessageAsync(
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(jobId)));

            await _tableService.CreateAsync(
                new Model.JobStatus(
                    jobId.Split('-')[0], 
                    jobId.Split('-')[1], 
                    false)
                );
            
            return CreateResponse(
                req,
                $"Here is your personal Job ID (To be used for fetching the images) :{jobId}",
                HttpStatusCode.OK);
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
