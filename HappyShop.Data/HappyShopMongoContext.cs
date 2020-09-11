using HappyShop.Comm;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    class HappyShopMongoContext : IHappyShopMongoContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IMongoDatabase Database { get; private set; }

        /// <summary>
        /// MongodbConfig
        /// </summary>
        /// <param name="configAccessor"></param>
        public HappyShopMongoContext(IOptionsMonitor<AppConfig> options)
        {
            var client = new MongoClient(options.CurrentValue.MongodbConfig.ConnectionString);
            Database = client.GetDatabase(options.CurrentValue.MongodbConfig.DatabaseName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="collectionName">集合名称,
        /// 如果集合名称为空，就取<typeparamref name="TDocument"/>的CollectionNameAttribute
        /// 否则就取类名，忽略Entity结束名
        ///  </param>
        /// <returns></returns>
        public IMongoCollection<TDocument> Collection<TDocument>(string collectionName = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                collectionName = typeof(TDocument).Name;
                if (collectionName.EndsWith("Entity"))
                {
                    collectionName = Regex.Replace(collectionName, "Entity$", "");
                }
                if (collectionName.EndsWith("Document"))
                {
                    collectionName = Regex.Replace(collectionName, "Document$", "");
                }
            }
            return Database.GetCollection<TDocument>(collectionName);
        }
    }
}
