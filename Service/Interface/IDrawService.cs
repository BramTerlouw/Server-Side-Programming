using JobQueueTrigger.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobQueueTrigger.Service.Interface
{
    public interface IDrawService
    {
        void getWeatherImage(string jsonImage, StationMeasurement measurement);
    }
}
