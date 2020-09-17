using HappyShop.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserInfoData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Task<UserInfoEntity> GetUserByAccount(string accountName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfoEntity> GetUserById(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserInfoEntity> SaveUpdate(UserInfoEntity user);
    }
}
