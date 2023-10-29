using ServerSideProgramming.Model.Entity;

namespace JobQueueTrigger.Service.Interface
{
    public interface IDrawService
    {
        byte[] DrawImage(byte[] byteArr, StationMeasurement measurement);
    }
}
