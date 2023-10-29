using ServerSideProgramming.Model.Entity;

namespace JobQueueTrigger.Service.Interface
{
    public interface IWeatherService
    {
        Task<StationMeasurement[]> GetWeather();
    }
}
