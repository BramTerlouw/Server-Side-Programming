using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using ServerSideProgramming.Model;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Service.Storage
{
    public class QueueService : IQueueService
    {

        private readonly QueueServiceClient _queueServiceClient;
        private QueueClient _queueClient;

        public QueueService()
        {
            _queueServiceClient = new QueueServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        }

        public void InitQueue(string queueName)
        {
            _queueClient = _queueServiceClient.GetQueueClient(queueName);
            if (!_queueClient.Exists())
            {
                _queueClient.Create();
            }
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
    }
}
