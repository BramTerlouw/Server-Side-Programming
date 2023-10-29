using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideProgramming.Model
{
    public class JobStatus : TableEntity
    {
        public string JobId { get; set; }
        public string JobName { get; set; }
        public bool IsCompleted { get; set; }

        public JobStatus() { }

        public JobStatus(string jobId, string jobName, bool isCompleted)
        {
            JobId = jobId;
            JobName = jobName;
            IsCompleted = isCompleted;

            PartitionKey = jobId;
            RowKey = jobId + jobName;
        }
    }
}
