using Azure.Storage.Queues.Models;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
            _logger = logger;
            _drawService = drawService;
            _blobService = blobService;
        }

        [Function(nameof(WriteQueueTrigger))]
        public void Run([QueueTrigger("writes", Connection = "writes-conn")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            // Draw measurement on the image
            //_drawService.getWeatherImage(Image, measurement);


            // Confert image to blob
            // ...


            // Add to blob with id
            //_blobService.CreateBlob(blobImage);
        }
    }
}
