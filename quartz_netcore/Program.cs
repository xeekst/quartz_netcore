using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using quartz_netcore.Jobs;
using quartz_netcore.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace quartz_netcore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Monitor quartz = null;
            var host = new HostBuilder()

               .ConfigureHostConfiguration(configHost =>
               {
                   configHost.AddEnvironmentVariables("ASPNETCORE_");
               })

               .ConfigureServices((ctx, services) =>
               {
                   services.AddSingleton<PrintJob>();
                   services.AddSingleton<IPrintInfo, PrintInfo>();
                   quartz = new Monitor(services.BuildServiceProvider(), ctx.Configuration);
                   
               })
               .ConfigureAppConfiguration((ctx, configApp) =>
               {
                   configApp.SetBasePath(Directory.GetCurrentDirectory());
                   configApp.AddJsonFile("appsettings.json", optional: true);
                   configApp.AddJsonFile("quartz_jobs.json", optional: true);
               })

               .UseConsoleLifetime()
               .Build();
            await quartz.StartAsync();
            await host.RunAsync();
            Console.WriteLine("Hello World!");
        }
    }
}
