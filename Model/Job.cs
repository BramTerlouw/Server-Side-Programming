using JobQueueTrigger.Model;

namespace ServerSideProgramming.Model
{
    public class Job
    {
        public string? JobId { get; set; }
        public StationMeasurement  Measurement { get; set; }
        public bool FinalJob { get; set; }

        public Job(string jobData, StationMeasurement measurement, bool finalJob)
        {
            this.JobId          = jobData;
            this.Measurement    = measurement;
            this.FinalJob       = finalJob;
        }
    }
}
