using HappyShop.Model;
using HappyShop.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserInfo> LoginAsync(LoginRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserInfo> RegisterAsync(LoginRequest request);
    }
}
