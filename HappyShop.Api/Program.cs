using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyShop.Data;
using HappyShop.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HappyShop.Api
{
    public class Program
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var hostBuilber = CreateHostBuilder(args);
            var host = hostBuilber.Build();
            //程序启动时执行初始化
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IMigrateService>();
                await db.InitDataAsync();
            }
            host.Run();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseUrls("http://*:5010");
                    webBuilder.UseStartup<Startup>();
                });
    }
}