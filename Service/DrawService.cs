using JobQueueTrigger.Model;
using JobQueueTrigger.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Drawing.Imaging;

namespace JobQueueTrigger.Service
{
    public class DrawService : IDrawService
    {
        private readonly ILogger<DrawService> _logger;

        public DrawService(ILogger<DrawService> logger)
        {
            _logger = logger;
        }

        public void getWeatherImage(string jsonImage, StationMeasurement measurement)
        {
            Bitmap bitMapImage = new(jsonImage);

            Graphics Gimg = Graphics.FromImage(bitMapImage);
            Font imgFont = new Font("Arial", 12);

            PointF pointLineOne = new PointF(5, 5);
            PointF pointLineTwo = new PointF(5, 20);
            PointF pointLineThree = new PointF(5, 35);

            Color color = Color.White;
            SolidBrush bForeColor = new SolidBrush(color);

            Gimg.DrawString($"", imgFont, bForeColor, pointLineOne);
            Gimg.DrawString($"", imgFont, bForeColor, pointLineTwo);
            Gimg.DrawString($"", imgFont, bForeColor, pointLineThree);

            bitMapImage.Save("imagePath", ImageFormat.Jpeg);
        }
    }
}
