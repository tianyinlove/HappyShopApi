using HappyShop.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IHappyShopMongoContext
    {
        /// <summary>
        ///
        /// </summary>
        IMongoCollection<UserInfoDocument> UserInfo { get; }

        /// <summary>
        ///
        /// </summary>
        IMongoCollection<MyFollowInfoDocument> MyFollowInfo { get; }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="collectionName">集合名称,
        /// 如果集合名称为空，就取<typeparamref name="TDocument"/>的CollectionNameAttribute
        /// 否则就取类名，忽略Entity结束名
        ///  </param>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName = null);

        /// <summary>
        ///
        /// </summary>
        void InitUserInfoIndexs();
    }
}