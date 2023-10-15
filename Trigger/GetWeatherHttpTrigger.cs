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

        [Function("GetWeatherHttpTrigger")]
        public CustomOutputType Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string jobId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();


            var response = req.CreateResponse(HttpStatusCode.OK);
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
