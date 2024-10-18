using HappyShop.Documents;
using HappyShop.Entity;
using HappyShop.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HappyShop.Data
{
    /// <summary>
    ///
    /// </summary>
    internal class MyFollowData : IMyFollowData
    {
        private IHappyShopMongoContext _mongoContext;

        /// <summary>
        ///
        /// </summary>
        public MyFollowData(IHappyShopMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        /// <summary>
        ///
        /// </summary>
        public async Task<List<MyFollowInfoDocument>> GetMyFollows(string userName)
        {
            var filters = new List<FilterDefinition<MyFollowInfoDocument>>();
            filters.Add(Builders<MyFollowInfoDocument>.Filter.Where(x => x.IsFollow == true));
            if (!string.IsNullOrEmpty(userName))
            {
                filters.Add(Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userName));
            }
            return await _mongoContext.MyFollowInfo.Find(Builders<MyFollowInfoDocument>.Filter.And(filters)).ToListAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdate(string userName, string stockPool, string stockCode, bool isFollow)
        {
            FilterDefinition<MyFollowInfoDocument> filter;
            if (string.IsNullOrEmpty(stockCode))
            {
                filter = Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userName && x.StockPool == stockPool && string.IsNullOrEmpty(x.StockCode));
            }
            else
            {
                filter = Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userName && x.StockPool == stockPool && x.StockCode == stockCode);
            }

            var update = Builders<MyFollowInfoDocument>.Update
                    .SetOnInsert(d => d.Id, Guid.NewGuid().ToString())
                    .SetOnInsert(d => d.UserName, userName)
                    .SetOnInsert(d => d.StockPool, stockPool)
                    .SetOnInsert(d => d.StockCode, stockCode)
                    .SetOnInsert(d => d.CreateTime, DateTime.Now)
                    .Set(d => d.IsFollow, isFollow)
                    .Set(d => d.UpdateTime, DateTime.Now);

            var options = new FindOneAndUpdateOptions<MyFollowInfoDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            var result = await _mongoContext.MyFollowInfo.FindOneAndUpdateAsync(filter, update, options);
            return result.Id != null;
        }
    }
}