using HappyShop.Domian;
using HappyShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    public interface IQYUserInfoService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<QYUserInfo> GetUserByUserIdAsync(string userId, int accountId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QYUserInfo> GetUserByIdAsync(string id);

        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<QYUserInfo> LoginAsync(string code, int accountId);
    }
}