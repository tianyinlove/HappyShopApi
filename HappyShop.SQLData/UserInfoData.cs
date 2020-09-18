using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Domian;
using HappyShop.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    class UserInfoData : IUserInfoData
    {
        private HappyShopSQLContext _shopSQLContext;

        /// <summary>
        /// 
        /// </summary>
        public UserInfoData()
        {
            _shopSQLContext = new HappyShopSQLContext();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitData()
        {
            _shopSQLContext.EnsureMigrate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserByAccount(string accountName)
        {
            var result = await _shopSQLContext.UserInfo
                 .FirstOrDefaultAsync(x => x.UnionId == accountName || x.OpenId == accountName || x.PhoneNumber == accountName);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserById(string id)
        {
            var result = await _shopSQLContext.UserInfo.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> SaveUpdate(UserInfoEntity user)
        {
            var model = await _shopSQLContext.UserInfo.FirstOrDefaultAsync(x =>
                  (!string.IsNullOrEmpty(x.UnionId) && x.UnionId == user.UnionId) ||
                  (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber == user.PhoneNumber) ||
                  (!string.IsNullOrEmpty(x.OpenId) && x.OpenId == user.OpenId)
             );
            if (model != null)
            {
                model.HeadImg = user.HeadImg;
                model.NickName = user.NickName;
                model.OpenId = user.OpenId;
                model.PassWord = user.PassWord;
                model.PhoneNumber = user.PhoneNumber;
                model.UnionId = user.UnionId;
                model.UpdateTime = DateTime.Now;
                _shopSQLContext.UserInfo.Update(model);
            }
            else
            {
                model = user;
                model.Id = Guid.NewGuid().ToString();
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                _shopSQLContext.UserInfo.Add(model);
            }
            await _shopSQLContext.SaveChangesAsync();
            return model;
        }
    }
}
