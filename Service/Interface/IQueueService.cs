namespace ServerSideProgramming.Service.Interface
{
    public interface IQueueService
    {
        void InitQueue(string queueName);
        Task SendMessageAsync(string message);
    }
}
