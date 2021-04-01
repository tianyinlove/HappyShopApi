using HappyShop.Comm;
using HappyShop.Documents;
using HappyShop.Domian;
using HappyShop.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.NetLog;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    class UserInfoData : IUserInfoData
    {
        private HappyShopMongoContext _mongoContext;
        private IMongoCollection<UserInfoDocument> _userInfo;

        /// <summary>
        /// 
        /// </summary>
        public UserInfoData(IOptionsMonitor<AppConfig> options)
        {
            _mongoContext = new HappyShopMongoContext(options);
            _userInfo = _mongoContext.Collection<UserInfoDocument>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitData()
        {
            Logger.WriteLog(LogLevel.Trace, "初始化索引");
            _mongoContext.InitUserInfoIndexs();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserByAccountAsync(string accountName)
        {
            return await GetUserByAccount(accountName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserByAccount(string accountName)
        {
            var filter = Builders<UserInfoDocument>.Filter.Or(
                Builders<UserInfoDocument>.Filter.Where(x => x.UnionId == accountName),
                Builders<UserInfoDocument>.Filter.Where(x => x.OpenId == accountName),
                Builders<UserInfoDocument>.Filter.Regex(x => x.PhoneNumber, new BsonRegularExpression(new Regex(accountName, RegexOptions.IgnoreCase))));

            var result = await _userInfo.Find(filter).FirstOrDefaultAsync();
            return result.Convert<UserInfoDocument, UserInfoEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserById(string id)
        {
            var result = await _userInfo.Find(x => x.Id == id).FirstOrDefaultAsync();
            return result.Convert<UserInfoDocument, UserInfoEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> SaveUpdate(UserInfoEntity user)
        {
            var filters = new List<FilterDefinition<UserInfoDocument>>();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                filters.Add(Builders<UserInfoDocument>.Filter.Regex(x => x.PhoneNumber, new BsonRegularExpression(new Regex(user.PhoneNumber, RegexOptions.IgnoreCase))));
            }
            if (!string.IsNullOrEmpty(user.UnionId))
            {
                filters.Add(Builders<UserInfoDocument>.Filter.Where(x => x.UnionId == user.UnionId));
            }
            if (!string.IsNullOrEmpty(user.OpenId))
            {
                filters.Add(Builders<UserInfoDocument>.Filter.Where(x => x.OpenId == user.OpenId));
            }

            var filter = Builders<UserInfoDocument>.Filter.Or(filters);

            var update = Builders<UserInfoDocument>.Update
                    .SetOnInsert(d => d.Id, Guid.NewGuid().ToString())
                    .SetOnInsert(d => d.CreateTime, DateTime.Now)
                    .Set(d => d.HeadImg, user.HeadImg)
                    .Set(d => d.NickName, user.NickName)
                    .Set(d => d.OpenId, user.OpenId)
                    .Set(d => d.PassWord, user.PassWord)
                    .Set(d => d.PhoneNumber, user.PhoneNumber)
                    .Set(d => d.UnionId, user.UnionId)
                    .Set(d => d.UpdateTime, DateTime.Now);

            var options = new FindOneAndUpdateOptions<UserInfoDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            var result = await _userInfo.FindOneAndUpdateAsync(filter, update, options);
            return result.Convert<UserInfoDocument, UserInfoEntity>();
        }
    }
}
