using Microsoft.WindowsAzure.Storage.Table;
using ServerSideProgramming.Model.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideProgramming.Model.Entity
{
    public class JobStatus : TableEntity
    {
        public string Requested_At { get; set; }
        public string Name { get; set; }
        public int Status {  get; set; }

        public JobStatus() { }

        public JobStatus(string requested_At, string name, int status)
        {
            Requested_At = requested_At;
            Name = name;
            Status = status;

            PartitionKey = requested_At;
            RowKey = requested_At + name;
        }
    }
}
