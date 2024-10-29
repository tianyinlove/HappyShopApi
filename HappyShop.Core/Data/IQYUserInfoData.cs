using HappyShop.Documents;
using HappyShop.Entity;
using HappyShop.Model;
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
    public interface IQYUserInfoData
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<QYUserInfo> GetUserByUserIdAsync(string userId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QYUserInfo> GetUserByIdAsync(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<QYUserInfo> SaveUpdateAsync(QYUserInfo user);
    }
}