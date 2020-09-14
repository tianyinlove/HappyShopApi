using HappyShop.Comm;
using Microsoft.Extensions.DependencyInjection;
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
            var _assembly = Assembly.GetExecutingAssembly();
            return services.AddAssembly(_assembly);
        }

    }
}
