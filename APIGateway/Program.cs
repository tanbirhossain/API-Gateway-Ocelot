using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
             .UseKestrel()
             .UseContentRoot(Directory.GetCurrentDirectory())
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config
                     .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                     .AddJsonFile("appsettings.json", true, true)
                     .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                     .AddJsonFile("ocelot.json")
                     .AddEnvironmentVariables();
             })
             .ConfigureServices(s =>
             {
                 s.AddOcelot();
             })
             .ConfigureLogging((hostingContext, logging) =>
             {
                 //add your logging
             })
             .UseIIS()
             .Configure(app =>
             {
                 app.UseOcelot().Wait();
             })
             .Build()
             .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((host, config) =>
            {

                config
                    .SetBasePath(host.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json", true, true)
                    .AddEnvironmentVariables();
            })
            .UseStartup<Startup>();
    }
}
