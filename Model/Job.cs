using JobQueueTrigger.Model;

namespace ServerSideProgramming.Model
{
    public class Job
    {
        public string? JobId { get; set; }
        public string? Image { get; set; }
        public StationMeasurement? Measurement { get; set; }

        public Job(string jobData, string image, StationMeasurement measurement)
        {
            this.JobId          = jobData;
            this.Image          = image;
            this.Measurement    = measurement;
        }
    }
}
