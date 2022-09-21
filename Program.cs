using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using vmProjectBFF.Services;

namespace vmProjectBFF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Configures logging for the project. To use in each class, use the following template
            // as a class level method as shown below. Then just use Logger any time to log a message in that class.
            // ILogger Logger { get; } = AppLogger.CreateLogger<*ClassName*>();
            ILoggerFactory LoggerFactory = services.GetRequiredService<ILoggerFactory>();
            AppLogger.LoggerFactory = LoggerFactory;
            BackgroundService1 bs1 = services.GetRequiredService<BackgroundService1>();
            bs1.ReadAndUpdateDB();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<BackgroundService1>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}