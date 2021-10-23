using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageLayer.Services;
using StorageLayer.Worker;
using StorageLayer.Models;
using Microsoft.Extensions.Options;

namespace StorageLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            var a = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //            services.AddSqsCustomerService(hostContext.Configuration);
        //            var dbSection = hostContext.Configuration.GetSection(nameof(DatabaseSettings));
        //            services.Configure<DatabaseSettings>(dbSection);
        //            services.AddSingleton(sp =>sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        //            services.AddSingleton<CustomerContext>();
        //            services.AddSingleton<CustomerStorageService>();
        //            services.AddHostedService<ListenSqsService>();

        //        });
    }
}
