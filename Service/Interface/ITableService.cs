using ServerSideProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideProgramming.Service.Interface
{
    public interface ITableService
    {
        void InitTable(string tableName);
        Task CreateAsync(JobStatus entity);
        Task UpdateRecordInTable(string jobId, string jobName);
        Task<JobStatus> RetrieveRecord(string partitionKey, string rowKey);
    }
}
