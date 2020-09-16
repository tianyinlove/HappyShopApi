using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utility.Extensions;

namespace HappyShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add framework services.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //◊¢≤·±æµÿ≈‰÷√
            services.Configure<AppConfig>(Configuration);
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddHappyShopService();
            services.AddHappyShopData();
            services.AddControllers();
            services.AddMvc();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseException();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
