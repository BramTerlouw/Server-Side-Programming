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
        public string Requested_At { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }

        public JobStatus() { }

        public JobStatus(string requested_At, string name, bool isCompleted)
        {
            Requested_At = requested_At;
            Name = name;
            IsCompleted = isCompleted;

            PartitionKey = requested_At;
            RowKey = requested_At + name;
        }
    }
}
