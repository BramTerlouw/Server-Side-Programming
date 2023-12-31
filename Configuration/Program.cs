using JobQueueTrigger.Service;
using JobQueueTrigger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerSideProgramming.Service;
using ServerSideProgramming.Service.Interface;
using ServerSideProgramming.Service.Storage;

var host = new HostBuilder()
      .ConfigureAppConfiguration(configurationBuilder =>
      {
      })
      .ConfigureFunctionsWorkerDefaults()
      .ConfigureServices(services =>
      {
          services.AddTransient<IWeatherService, WeatherService>();
          services.AddTransient<IDrawService, DrawService>();
          services.AddTransient<IDownloadImageService, DownloadImageService>();
          services.AddTransient<IQueueService, QueueService>();
          services.AddTransient<IBlobService, BlobService>();
          services.AddTransient<ITableService, TableService>();
      })
            .Build();

host.Run();
