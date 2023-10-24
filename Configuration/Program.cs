using JobQueueTrigger.Service;
using JobQueueTrigger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerSideProgramming.Service;
using ServerSideProgramming.Service.Interface;

var host = new HostBuilder()
      .ConfigureAppConfiguration(configurationBuilder =>
      {
      })
      .ConfigureFunctionsWorkerDefaults()
      .ConfigureServices(services =>
      {
          services.AddTransient<IWeatherService, WeatherService>();
          services.AddTransient<IDrawService, DrawService>();
          services.AddTransient<IBlobService, BlobService>();
          services.AddTransient<IDownloadImageService, DownloadImageService>();
          services.AddTransient<IQueueService, QueueService>();
      })
            .Build();

host.Run();
