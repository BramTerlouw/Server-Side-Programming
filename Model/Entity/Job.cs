namespace ServerSideProgramming.Model.Entity
{
    public class Job
    {
        public string? JobId { get; set; }
        public StationMeasurement Measurement { get; set; }
        public bool FinalJob { get; set; }

        public Job(string jobData, StationMeasurement measurement, bool finalJob)
        {
            JobId = jobData;
            Measurement = measurement;
            FinalJob = finalJob;
        }
    }
}
