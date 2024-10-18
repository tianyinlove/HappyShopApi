using HappyShop.Comm;
using HappyShop.Documents;
using HappyShop.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HappyShop.Data
{
    /// <summary>
    ///
    /// </summary>
    internal class HappyShopMongoContext : IHappyShopMongoContext
    {
        /// <summary>
        ///
        /// </summary>
        protected MongoClient Client { get; private set; }

        /// <summary>
        ///
        /// </summary>
        protected IMongoDatabase Database { get; private set; }

        /// <summary>
        /// MongodbConfig
        /// </summary>
        /// <param name="configAccessor"></param>
        public HappyShopMongoContext(IOptionsMonitor<AppConfig> options)
        {
            Client = new MongoClient(options.CurrentValue.MongodbConfig.ConnectionString);
            Database = Client.GetDatabase(options.CurrentValue.MongodbConfig.DatabaseName);
        }

        /// <summary>
        ///
        /// </summary>
        public IMongoCollection<UserInfoDocument> UserInfo => GetCollection<UserInfoDocument>("UserInfo");

        /// <summary>
        ///
        /// </summary>
        public IMongoCollection<MyFollowInfoDocument> MyFollowInfo => GetCollection<MyFollowInfoDocument>("MyFollowInfo");

        /// <summary>
        ///
        /// </summary>
        public void InitUserInfoIndexs()
        {
            var collection = GetCollection<UserInfoDocument>();
            var indexes = collection.Indexes.List().ToList();
            if (!indexes.Any(d => d.GetElement("name").Value.AsString == "PhoneNumber"))
            {
                collection.Indexes.CreateOne(new CreateIndexModel<UserInfoDocument>(
                    Builders<UserInfoDocument>.IndexKeys.Ascending(d => d.PhoneNumber),
                    new CreateIndexOptions
                    {
                        Name = "PhoneNumber",
                        Background = true
                    }));
            }

            if (!indexes.Any(d => d.GetElement("name").Value.AsString == "UnionId"))
            {
                collection.Indexes.CreateOne(new CreateIndexModel<UserInfoDocument>(
                    Builders<UserInfoDocument>.IndexKeys.Ascending(d => d.UnionId),
                    new CreateIndexOptions
                    {
                        Name = "UnionId",
                        Background = true
                    }));
            }

            if (!indexes.Any(d => d.GetElement("name").Value.AsString == "OpenId"))
            {
                collection.Indexes.CreateOne(new CreateIndexModel<UserInfoDocument>(
                    Builders<UserInfoDocument>.IndexKeys.Ascending(d => d.OpenId),
                    new CreateIndexOptions
                    {
                        Name = "OpenId",
                        Background = true
                    }));
            }
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
        public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName = null)
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