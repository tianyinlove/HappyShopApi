using HappyShop.Documents;
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
        Task<UserInfoDocument> GetUserInfo(string accountName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserInfoDocument> SaveUpdate(UserInfoDocument user);
    }
}
