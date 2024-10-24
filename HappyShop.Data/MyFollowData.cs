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
        /// <param name="userId">用户在企业内的UserID</param>
        /// <returns></returns>
        public async Task<List<MyFollowInfoDocument>> GetMyFollows(string userId)
        {
            var filters = new List<FilterDefinition<MyFollowInfoDocument>>();
            filters.Add(Builders<MyFollowInfoDocument>.Filter.Where(x => x.IsFollow == true));
            if (!string.IsNullOrEmpty(userId))
            {
                filters.Add(Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userId));
            }
            return await _mongoContext.MyFollowInfo.Find(Builders<MyFollowInfoDocument>.Filter.And(filters)).ToListAsync();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <returns></returns>
        public async Task<bool> SaveUpdate(string userId, string stockPool, string stockCode, bool isFollow)
        {
            FilterDefinition<MyFollowInfoDocument> filter;
            if (!string.IsNullOrEmpty(stockPool) && string.IsNullOrEmpty(stockCode))
            {
                filter = Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userId && x.StockPool == stockPool && string.IsNullOrEmpty(x.StockCode));
            }
            else if (string.IsNullOrEmpty(stockPool) && !string.IsNullOrEmpty(stockCode))
            {
                filter = Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userId && string.IsNullOrEmpty(x.StockPool) && x.StockCode == stockCode);
            }
            else
            {
                filter = Builders<MyFollowInfoDocument>.Filter.Where(x => x.UserName == userId && x.StockPool == stockPool && x.StockCode == stockCode);
            }

            var update = Builders<MyFollowInfoDocument>.Update
                    .SetOnInsert(d => d.Id, Guid.NewGuid().ToString())
                    .SetOnInsert(d => d.UserName, userId)
                    .SetOnInsert(d => d.CreateTime, DateTime.Now)
                    .Set(d => d.StockPool, stockPool)
                    .Set(d => d.StockCode, stockCode)
                    .Set(d => d.IsFollow, isFollow)
                    .Set(d => d.UpdateTime, DateTime.Now);

            var options = new FindOneAndUpdateOptions<MyFollowInfoDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            var result = await _mongoContext.MyFollowInfo.FindOneAndUpdateAsync(filter, update, options);
            return result.Id != null;
        }
    }
}