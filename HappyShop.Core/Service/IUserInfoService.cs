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
        Task<UserInfo> LoginAsync(WechatRequest request);

        /// <summary>
        /// 注册/修改用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserInfo> SaveUpdateAsync(UserReuqest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acountId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<WechatJSTicket> GetJsApiSign(int acountId, string url);
    }
}
