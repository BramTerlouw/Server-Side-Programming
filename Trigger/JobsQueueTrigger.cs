using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using JobQueueTrigger.Model;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model;
using ServerSideProgramming.Service.Interface;
using System.Text;

namespace ServerSideProgramming.Trigger
{
    public class JobsQueueTrigger
    {
        private readonly ILogger<JobsQueueTrigger> _logger;
        private readonly IWeatherService _weatherService;
        private readonly IFetchImageService _fetchImageService;
        private readonly IDownloadImageService _downloadImageService;

        public JobsQueueTrigger(
            ILogger<JobsQueueTrigger> logger,
            IWeatherService weatherService,
            IFetchImageService fetchImageService,
            IDownloadImageService downloadImageService)
        {
            _logger                 = logger;
            _weatherService         = weatherService;
            _fetchImageService      = fetchImageService;
            _downloadImageService   = downloadImageService;
        }

        [Function(nameof(JobsQueueTrigger))]
        public async Task RunAsync([QueueTrigger("jobs", Connection = "jobs-conn")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed Job with ID: {message.MessageText}");
            QueueClient queueClient = InitializeQueueClient();

            string jobId = message.Body.ToString();
            StationMeasurement[] measurements = await _weatherService.GetWeather();
            //string imageUrl = await _fetchImageService.FetchUrl();
            //Image image = _downloadImageService.getImageFromUrl(imageUrl);

            for (int i = 0; i < 2; i++)
            {
                Job data = new Job(jobId, "image", measurements[i]);
                await queueClient.SendMessageAsync(
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(data)
                        )
                    )
                );
            }
        }

        private QueueClient InitializeQueueClient()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var queueServiceClient = new QueueServiceClient(connectionString);
            var queueClient = queueServiceClient.GetQueueClient("writes");

            if (!queueClient.Exists())
            {
                queueClient.Create();
            }
            return queueClient;
        }
    }
}
