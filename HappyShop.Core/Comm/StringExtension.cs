using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HappyShop.Comm
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection Add(this Assembly assembly, IServiceCollection services)
        {
            AppDomain.CurrentDomain.GetAssemblies().Where(o => o.Equals(assembly)).ToList().ForEach(o =>
            {
                o.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsSealed).ToList().ForEach(t =>
                {
                    var iface = t.GetInterfaces();
                    if (iface != null && iface.Length > 0)
                    {
                        var ifaceLst = iface.ToList();
                        ifaceLst.ForEach(It =>
                        {
                            if (It != null)
                            {
                                services.AddTransient(It, t);
                            }
                        });
                    }
                });
            });
            return services;
        }
    }
}
