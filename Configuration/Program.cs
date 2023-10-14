using JobQueueTrigger.Service;
using JobQueueTrigger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
      .ConfigureAppConfiguration(configurationBuilder =>
      {
      })
      .ConfigureFunctionsWorkerDefaults()
      .ConfigureServices(services =>
      {
          services.AddTransient<IWeatherService, WeatherService>();
          services.AddTransient<IFetchImageService, FetchImageService>();
          services.AddTransient<IDrawService, DrawService>();
          services.AddTransient<IBlobService, BlobService>();
      })
            .Build();

host.Run();
