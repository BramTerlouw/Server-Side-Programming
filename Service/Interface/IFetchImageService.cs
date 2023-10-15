using System.Drawing;

namespace JobQueueTrigger.Service.Interface
{
    public interface IFetchImageService
    {
        Task<string> FetchUrl();
    }
}
