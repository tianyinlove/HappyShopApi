﻿using HappyShop.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utility.Extensions;

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
            services.TryAddScoped<IUserInfoData, UserInfoData>();
            return services;
        }
    }
}