using JobQueueTrigger.Model;

namespace JobQueueTrigger.Service.Interface
{
    public interface IDrawService
    {
        void getWeatherImage(string jsonImage, StationMeasurement measurement);
    }
}
