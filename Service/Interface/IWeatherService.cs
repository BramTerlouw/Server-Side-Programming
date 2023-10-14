using JobQueueTrigger.Model;

namespace JobQueueTrigger.Service.Interface
{
    public interface IWeatherService
    {
        Task<StationMeasurement[]> GetWeather();
    }
}
