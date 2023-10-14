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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            // Creates a job and puts it into queue
            // ...


            // Returns a jobid ??
            // ...


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        public class CustomOutputType
        {
            [QueueOutput("jobs")]
            public string? JobId { get; set; }
        }
    }
}
