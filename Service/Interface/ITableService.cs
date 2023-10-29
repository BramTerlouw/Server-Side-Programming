using ServerSideProgramming.Model.Entity;
using ServerSideProgramming.Model.Enumeration;
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
        Task UpdateRecordInTable(string jobId, string jobName, StatusType status);
        Task<JobStatus> RetrieveRecord(string partitionKey, string rowKey);
    }
}
