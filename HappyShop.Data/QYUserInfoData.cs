using HappyShop.Comm;
using HappyShop.Documents;
using HappyShop.Entity;
using HappyShop.Model;
using HappyShop.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack;
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
    internal class QYUserInfoData : IQYUserInfoData
    {
        private readonly IHappyShopMongoContext _mongoContext;
        private readonly ILogger<QYUserInfoData> _logger;

        /// <summary>
        ///
        /// </summary>
        public QYUserInfoData(IHappyShopMongoContext mongoContext,
            ILogger<QYUserInfoData> logger)
        {
            _mongoContext = mongoContext;
            this._logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> GetUserByUserIdAsync(string userId)
        {
            var filter = Builders<QYUserInfoDocument>.Filter.Where(x => x.UserId == userId);

            var result = await _mongoContext.QYUserInfo.Find(filter).FirstOrDefaultAsync();
            return result.Convert<QYUserInfoDocument, QYUserInfo>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> GetUserByIdAsync(string id)
        {
            var result = await _mongoContext.QYUserInfo.Find(x => x.Id == id).FirstOrDefaultAsync();
            return result.Convert<QYUserInfoDocument, QYUserInfo>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> SaveUpdateAsync(QYUserInfo user)
        {
            var filters = new List<FilterDefinition<QYUserInfoDocument>>();
            if (!string.IsNullOrEmpty(user.UserId))
            {
                filters.Add(Builders<QYUserInfoDocument>.Filter.Where(x => x.UserId == user.UserId));
            }
            if (!string.IsNullOrEmpty(user.Open_UserId))
            {
                filters.Add(Builders<QYUserInfoDocument>.Filter.Where(x => x.Open_UserId == user.Open_UserId));
            }
            if (!string.IsNullOrEmpty(user.Mobile))
            {
                filters.Add(Builders<QYUserInfoDocument>.Filter.Where(x => x.Mobile == user.Mobile));
            }

            var filter = Builders<QYUserInfoDocument>.Filter.Or(filters);

            var update = Builders<QYUserInfoDocument>.Update
                    .SetOnInsert(d => d.Id, Guid.NewGuid().ToString())
                    .SetOnInsert(d => d.CreateTime, DateTime.Now)
                    .Set(d => d.UserId, user.UserId)
                    .Set(d => d.Open_UserId, user.Open_UserId)
                    .Set(d => d.Mobile, user.Mobile)
                    .Set(d => d.Name, user.Name)
                    .Set(d => d.Gender, user.Gender)
                    .Set(d => d.Email, user.Email)
                    .Set(d => d.Avatar, user.Avatar)
                    .Set(d => d.Telephone, user.Telephone)
                    .Set(d => d.Status, user.Status)
                    .Set(d => d.UpdateTime, DateTime.Now);

            var options = new FindOneAndUpdateOptions<QYUserInfoDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            var result = await _mongoContext.QYUserInfo.FindOneAndUpdateAsync(filter, update, options);
            return result.Convert<QYUserInfoDocument, QYUserInfo>();
        }
    }
}