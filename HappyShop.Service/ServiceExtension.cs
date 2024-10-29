using HappyShop.Comm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utility.Extensions;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHappyShopService(this IServiceCollection services)
        {
            services.TryAddScoped<IApiClient, ApiClient>();
            services.TryAddScoped<IOAuthService, OAuthService>();
            services.TryAddScoped<IStockMonitorService, StockMonitorService>();
            services.TryAddScoped<IUserInfoService, UserInfoService>();
            services.TryAddScoped<IWeChatService, WeChatService>();
            services.TryAddScoped<IMyFollowService, MyFollowService>();
            services.TryAddScoped<IQYUserInfoService, QYUserInfoService>();
            services.TryAddScoped<IMigrateService, MigrateService>();
            return services;
        }
    }
}