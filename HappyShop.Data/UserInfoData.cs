using HappyShop.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    class UserInfoData : IUserInfoData
    {
        private IMongoCollection<UserInfoDocument> _userInfo;

        /// <summary>
        /// 
        /// </summary>
        public UserInfoData(IHappyShopMongoContext mongoContext)
        {
            _userInfo = mongoContext.Collection<UserInfoDocument>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<UserInfoDocument> GetUserInfo(string accountName)
        {
            var filter = Builders<UserInfoDocument>.Filter.Or(
                Builders<UserInfoDocument>.Filter.Where(x => x.UnionId == accountName),
                Builders<UserInfoDocument>.Filter.Where(x => x.OpenId == accountName),
                Builders<UserInfoDocument>.Filter.Regex(x => x.PhoneNumber, new BsonRegularExpression(new Regex(accountName, RegexOptions.IgnoreCase))),
                Builders<UserInfoDocument>.Filter.Regex(x => x.Email, new BsonRegularExpression(new Regex(accountName, RegexOptions.IgnoreCase))));

            return await _userInfo.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserInfoDocument> SaveUpdate(UserInfoDocument user)
        {
            var filters = new List<FilterDefinition<UserInfoDocument>>();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                filters.Add(Builders<UserInfoDocument>.Filter.Regex(x => x.PhoneNumber, new BsonRegularExpression(new Regex(user.PhoneNumber, RegexOptions.IgnoreCase))));
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                filters.Add(Builders<UserInfoDocument>.Filter.Regex(x => x.Email, new BsonRegularExpression(new Regex(user.Email, RegexOptions.IgnoreCase))));
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
                    .SetOnInsert(d => d.Id, Guid.NewGuid())
                    .SetOnInsert(d => d.CreateTime, DateTime.Now)
                    .Set(d => d.Email, user.Email)
                    .Set(d => d.HeadImg, user.HeadImg)
                    .Set(d => d.NickName, user.NickName)
                    .Set(d => d.OpenId, user.OpenId)
                    .Set(d => d.PassWord, user.PassWord)
                    .Set(d => d.PhoneNumber, user.PhoneNumber)
                    .Set(d => d.UnionId, user.UnionId)
                    .Set(d => d.UpdateTime, DateTime.Now);

            var options = new FindOneAndUpdateOptions<UserInfoDocument> { IsUpsert = true, ReturnDocument = ReturnDocument.After };

            return await _userInfo.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}
