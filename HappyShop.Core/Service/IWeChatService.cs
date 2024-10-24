using HappyShop.Domian;
using HappyShop.Request;
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
    public interface IWeChatService
    {
        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<QYWechatLoginUser> LoginAsync(string code);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <returns></returns>
        Task<QYWechatUserInfo> GetUserInfoAsync(string userId);

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="request">通知对象</param>
        /// <returns></returns>
        Task<bool> NoticeAsync(WechatMessageRequest request);
    }
}