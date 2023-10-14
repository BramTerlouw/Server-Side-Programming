using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ServerSideProgramming.Trigger
{
    public class FetchWeatherHttpTrigger
    {
        private readonly ILogger _logger;

        public FetchWeatherHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FetchWeatherHttpTrigger>();
        }

        [Function("FetchWeatherHttpTrigger")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            // Get status of job or list of images with weather data
            // ...


            // Use job id provided by other http trigger endpoint
            // ...


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
