using System;
using Hahn.ApplicationProcess.December2020.Web.Infrastructure;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hahn.ApplicationProcess.December2020.Web
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception exception)
            {
                Logging.Logger.Fatal(exception, "Could not start web app");
                return -1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new LightInjectServiceProviderFactory(DependencyInjectionContainer.Instance))
                .UseSerilog(Logging.ConfigureLogger)
                .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<CompositionRoot>();
                 });
    }
}