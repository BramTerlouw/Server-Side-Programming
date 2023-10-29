using Azure.Storage.Queues.Models;
using JobQueueTrigger.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerSideProgramming.Model.Entity;
using ServerSideProgramming.Model.Enumeration;
using ServerSideProgramming.Service.Interface;
using System.Text;

namespace ServerSideProgramming.Trigger
{
    public class JobsQueueTrigger
    {
        private readonly ILogger<JobsQueueTrigger> _logger;
        private readonly IWeatherService _weatherService;
        private readonly IQueueService _queueService;
        private readonly ITableService _tableService;

        public JobsQueueTrigger(
            ILogger<JobsQueueTrigger> logger,
            IWeatherService weatherService,
            IQueueService queueService,
            ITableService tableService)
        {
            _logger                 = logger;
            _weatherService         = weatherService;
            _queueService           = queueService;
            _tableService           = tableService;
        }


        /// <summary>
        /// Method RunAsync is a Azure Queue trigger which triggers on the "jobs" queue. The method fetches
        /// weather station information from an api, loops through it and add a measurement with the 
        /// job id to the writing queue.
        /// </summary>
        /// 
        /// <param name="message">
        /// Message contains a Job ID.
        /// </param>
        [Function(nameof(JobsQueueTrigger))]
        public async Task RunAsync([QueueTrigger("jobs", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed Job with ID: {message.MessageText}");
            _queueService.InitQueue("writes");
            _tableService.InitTable("jobstatus");


            string jobId = message.Body.ToString();
            StationMeasurement[] measurements = await _weatherService.GetWeather();


            await _tableService.UpdateRecordInTable(
                jobId.Split('-')[0], 
                jobId.Split('-')[1], 
                StatusType.Processing);


            for (int i = 0; i < measurements.Length; i++)
            {
                bool isLastItem = i == measurements.Length - 1;
                Job data = new Job(jobId, measurements[i], isLastItem);

                await _queueService.SendMessageAsync(
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(data))));
            }
        }
    }
}
