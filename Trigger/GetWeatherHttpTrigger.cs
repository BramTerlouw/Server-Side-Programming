using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ServerSideProgramming.Trigger
{
    public class GetWeatherHttpTrigger
    {
        private readonly ILogger _logger;

        public GetWeatherHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetWeatherHttpTrigger>();
        }


        /// <summary>
        /// Method Run is a Azure HTTP trigger which functions like an endpoint. This method initializes
        /// a new job by creating a job id and adding it to the queue.
        /// </summary>
        /// 
        /// <param name="req">
        /// Req is a HTTP GET request.
        /// </param>
        /// 
        /// <returns>
        /// Returns CustomOutputType obj as response, containing the job ID and HTTP response.
        /// </returns>
        [Function("GetWeatherHttpTrigger")]
        public CustomOutputType Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string jobId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString($"Here is your personal Job ID (To be used for fetching the images) :{jobId}");
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");


            string id = $"{jobId}";
            return new CustomOutputType
            {
                JobId = id,
                HttpResponse = response
            };
        }

        public class CustomOutputType
        {
            [QueueOutput("jobs")]
            public string? JobId { get; set; }
            public HttpResponseData? HttpResponse { get; set; }
        }
    }
}
