using Azure.Storage.Queues.Models;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model;

namespace ServerSideProgramming.Trigger
{
    public class WriteQueueTrigger
    {
        private readonly ILogger<WriteQueueTrigger> _logger;
        private readonly IDrawService _drawService;
        private readonly IBlobService _blobService;

        public WriteQueueTrigger(
            ILogger<WriteQueueTrigger> logger,
            IDrawService drawService,
            IBlobService blobService)
        {
            _logger         = logger;
            _drawService    = drawService;
            _blobService    = blobService;
        }

        [Function(nameof(WriteQueueTrigger))]
        public async Task RunAsync([QueueTrigger("writes", Connection = "writes-conn")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: ");

            Job? data = JsonConvert.DeserializeObject<Job>(message.Body.ToString());

            // Draw measurement on the image
            //_drawService.getWeatherImage(Image, measurement);


            // Confert image to blob
            string blob = JsonConvert.SerializeObject(data);


            // Add to blob with id
            await _blobService.InitBlobAsync("test");
            await _blobService.CreateBlob(blob);
        }
    }
}
