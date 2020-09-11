using System;
using System.Collections.Generic;
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
    }
}
