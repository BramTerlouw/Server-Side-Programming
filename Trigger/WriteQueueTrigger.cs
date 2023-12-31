using Azure.Storage.Queues.Models;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model.Entity;
using ServerSideProgramming.Model.Enumeration;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Trigger
{
    public class WriteQueueTrigger
    {
        private readonly ILogger<WriteQueueTrigger> _logger;
        private readonly IDownloadImageService _downloadImageService;
        private readonly IDrawService _drawService;
        private readonly IBlobService _blobService;
        private readonly ITableService _tableService;

        public WriteQueueTrigger(
            ILogger<WriteQueueTrigger> logger,
            IDownloadImageService downloadImageService,
            IDrawService drawService,
            IBlobService blobService,
            ITableService tableService)
        {
            _logger                 = logger;
            _downloadImageService   = downloadImageService;
            _drawService            = drawService;
            _blobService            = blobService;
            _tableService = tableService;
        }


        /// <summary>
        /// Method RunAsync is a Azure Queue trigger which triggers on the "writes" queue. The method downloads
        /// an iamge and draws stationmeasurements on it. It is then stored in blob.
        /// </summary>
        /// 
        /// <param name="message">
        /// Message contains a serialized StationMeasurement object.
        /// </param>
        [Function(nameof(WriteQueueTrigger))]
        public async Task RunAsync([QueueTrigger("writes", Connection = "AzureWebJobsStorage")] QueueMessage message)
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


            byte[] byteArr = await _downloadImageService.GetImage();
            byte[] writtenImage = _drawService.DrawImage(byteArr, data.Measurement);


            await _blobService.InitBlobAsync(data.JobId);
            await _blobService.CreateBlob($"{data.JobId}_{data.Measurement.stationname.Replace(" ", "_")}", writtenImage);


            if (data.FinalJob)
            {
                _tableService.InitTable("jobstatus");
                await _tableService.UpdateRecordInTable(data.JobId.Split('-')[0], data.JobId.Split('-')[1], StatusType.Finished);
            }
        }
    }
}
