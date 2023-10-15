using JobQueueTrigger.Model;

namespace ServerSideProgramming.Model
{
    public class Job
    {
        public string? JobId { get; set; }
        public StationMeasurement? Measurement { get; set; }

        public Job(string jobData, StationMeasurement measurement)
        {
            this.JobId          = jobData;
            this.Measurement    = measurement;
        }
    }
}
