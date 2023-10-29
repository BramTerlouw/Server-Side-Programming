using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServerSideProgramming.Model.Entity;
using ServerSideProgramming.Model.Enumeration;
using ServerSideProgramming.Service.Interface;

namespace ServerSideProgramming.Service.Storage
{
    public class TableService : ITableService
    {
        private readonly CloudStorageAccount _storageAccount;
        protected CloudTableClient _tableClient;
        protected CloudTable _table;


        public TableService()
        {
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            _tableClient = _storageAccount.CreateCloudTableClient();
        }

        public void InitTable(string tableName)
        {
            _table = _tableClient.GetTableReference(tableName);
            _table.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public async Task CreateAsync(JobStatus entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task UpdateRecordInTable(string jobId, string jobName, StatusType status)
        {
            JobStatus jobStatusEntity = await RetrieveRecord(jobId, jobId + jobName);
            if (jobStatusEntity is not null)
            {
                jobStatusEntity.Status = (int)status;
                TableOperation tableOperation = TableOperation.Replace(jobStatusEntity);
                await _table.ExecuteAsync(tableOperation);
            }
        }

        public async Task<JobStatus> RetrieveRecord(string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<JobStatus>(partitionKey, rowKey);
            TableResult tableResult = await _table.ExecuteAsync(tableOperation);
            return tableResult.Result as JobStatus;
        }
    }
}
