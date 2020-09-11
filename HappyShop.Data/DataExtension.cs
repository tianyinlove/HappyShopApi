using HappyShop.Comm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHappyShopData(this IServiceCollection services)
        {
            var _assembly = Assembly.GetExecutingAssembly();
            return _assembly.Add(services);
        }
    }
}
