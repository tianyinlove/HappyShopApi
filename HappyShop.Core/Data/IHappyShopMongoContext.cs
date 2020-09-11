using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHappyShopMongoContext
    {
        /// <summary>
        /// 
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<TDocument> Collection<TDocument>(string collectionName = null);
    }
}
