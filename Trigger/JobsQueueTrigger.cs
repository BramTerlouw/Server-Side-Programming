using Azure.Storage.Queues.Models;
using JobQueueTrigger.Model;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ServerSideProgramming.Trigger
{
    public class JobsQueueTrigger
    {
        private readonly ILogger<JobsQueueTrigger> _logger;
        private readonly IWeatherService _weatherService;
        private readonly IFetchImageService _fetchImageService;

        public JobsQueueTrigger(
            ILogger<JobsQueueTrigger> logger,
            IWeatherService weatherService,
            IFetchImageService fetchImageService)
        {
            _logger = logger;
            _weatherService = weatherService;
            _fetchImageService = fetchImageService;
        }

        [Function(nameof(JobsQueueTrigger))]
        public async Task RunAsync([QueueTrigger("jobs", Connection = "jobs-conn")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            // Get Job ID from QueueMessage
            string jobId = message.Body.ToString();

            // Get measurements and image for next job
            StationMeasurement[] measurements = await _weatherService.GetWeather();
            string image = await _fetchImageService.FetchImageAsync();

            // Create an image with weather data for each station
            // Add this job to a queue
            for (int i = 0; i < measurements.Length; i++)
            {
                new CustomOutputType
                {
                    JobId = jobId,
                    Image = image,
                    Measurement = measurements[i]
                };
            }
        }

        public class CustomOutputType
        {
            [QueueOutput("writes")]
            public string? JobId { get; set; }
            public string? Image { get; set; }
            public StationMeasurement? Measurement { get; set; }
        }
    }
}
