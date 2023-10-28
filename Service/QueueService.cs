using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using ServerSideProgramming.Model;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Service
{
    public class QueueService : IQueueService
    {

        private readonly QueueServiceClient _queueServiceClient;
        private QueueClient _queueClient;

        public QueueService()
        {
            _queueServiceClient = new QueueServiceClient("AzureWebJobsStorage");
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

        public async Task<bool> MessagesStillInQueue(string jobId)
        {
            QueueMessage[] receivedMessages = await _queueClient.ReceiveMessagesAsync();

            if (receivedMessages.Length != 0)
            {
                foreach (QueueMessage item in receivedMessages)
                {
                    Job job = JsonConvert.DeserializeObject<Job>(item.Body.ToString());
                    if (job.JobId ==  jobId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
