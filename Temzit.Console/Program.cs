using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Temzit.Api;
using Temzit.MQTT;

namespace Temzit.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                        .Configure<TemzitOptions>(hostContext.Configuration.GetSection("Temzit"))
                        .AddTemzit()
                        .AddMqtt();
                });
        }
    }
}