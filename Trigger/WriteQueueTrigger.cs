using Azure.Storage.Queues.Models;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model;
using ServerSideProgramming.Service;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Trigger
{
    public class WriteQueueTrigger
    {
        private readonly ILogger<WriteQueueTrigger> _logger;
        private readonly IFetchImageService _fetchImageService;
        private readonly IDownloadImageService _downloadImageService;
        private readonly IDrawService _drawService;
        private readonly IBlobService _blobService;

        public WriteQueueTrigger(
            ILogger<WriteQueueTrigger> logger,
            IFetchImageService fetchImageService,
            IDownloadImageService downloadImageService,
            IDrawService drawService,
            IBlobService blobService)
        {
            _logger                 = logger;
            _fetchImageService      = fetchImageService;
            _downloadImageService   = downloadImageService;
            _drawService            = drawService;
            _blobService            = blobService;
        }


        /// <summary>
        /// Method RunAsync is a Azure Queue trigger which triggers on the "writes" queue. The method fectches
        /// a url for an image, downloads it and draws stationmeasurements on it. It is then stored in blob.
        /// </summary>
        /// 
        /// <param name="message">
        /// Message contains a serialized StationMeasurement object.
        /// </param>
        [Function(nameof(WriteQueueTrigger))]
        public async Task RunAsync([QueueTrigger("writes", Connection = "writes-conn")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: ");
            Job? data = JsonConvert.DeserializeObject<Job>(message.Body.ToString());

            if (
                data == null || 
                data.JobId == null || 
                data.Measurement == null || 
                data.Measurement.stationname == null)
            {
                return;
            }


            string imageUrl = await _fetchImageService.FetchUrl();
            byte[] byteArr = _downloadImageService.getImageFromUrl(imageUrl);
            byte[] writtenImage = _drawService.DrawImage(byteArr, data.Measurement);


            await _blobService.InitBlobAsync(data.JobId);
            await _blobService.CreateBlob($"{data.JobId}_{data.Measurement.stationname.Replace(" ", "_")}", writtenImage);
        }
    }
}
