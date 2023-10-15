using JobQueueTrigger.Model;

namespace JobQueueTrigger.Service.Interface
{
    public interface IDrawService
    {
        byte[] DrawImage(byte[] byteArr, StationMeasurement measurement);
    }
}
