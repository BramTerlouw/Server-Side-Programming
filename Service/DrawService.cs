﻿using ImageMagick;
using JobQueueTrigger.Service.Interface;
using ServerSideProgramming.Model.Entity;

namespace JobQueueTrigger.Service
{
    public class DrawService : IDrawService
    {
        public byte[] DrawImage(byte[] byteArr, StationMeasurement measurement)
        {
            using MemoryStream stream   = new MemoryStream(byteArr);
            using MagickImage image     = new MagickImage(stream);

            image.Settings.FillColor        = MagickColors.Black;
            image.Settings.BorderColor      = MagickColors.Black;
            image.Settings.FontWeight       = FontWeight.Bold;
            image.Settings.FontPointsize    = 20;

            DrawableText stationName    = new DrawableText(50, 100, $"Station: {measurement.stationname}");
            DrawableText temperature    = new DrawableText(50, 130, $"Temperature: {measurement.temperature}");
            DrawableText windDir        = new DrawableText(50, 160, $"WindDirection: {measurement.winddirection}");
            DrawableText windSpeed      = new DrawableText(50, 190, $"Windspeed: {measurement.windspeed}");

            image.Draw(stationName);
            image.Draw(temperature);
            image.Draw(windDir);
            image.Draw(windSpeed);

            return image.ToByteArray();
        }
    }
}
