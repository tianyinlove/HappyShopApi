using HappyShop.Domian;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOAuthService
    {
        #region 公众号

        /// <summary>
        /// 微信获取code回调地址
        /// </summary>
        /// <param name="redirectUrl">redirectUrl</param>
        /// <param name="acountId"></param>
        /// <param name="isApp">是否是APP端</param>
        /// <returns></returns>
        string GetWeChatCode(string redirectUrl, int acountId, bool isApp = true);

        /// <summary>
        /// 根据code获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="acountId"></param>
        /// <returns></returns>
        Task<WeChatAccessTokenInfo> GetWeChatAccessTokenAsync(string code, int acountId);

        /// <summary>
        /// 根据accessToken和openId 获取用户信息
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        Task<WeChatUserInfo> GetWeChatUserInfoAsync(string accessToken, string openId);

        /// <summary>
        /// 公众号 根据accessToken获取jsapi_ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<WechatTicketInfo> GetWeChatTicketAsync(string accessToken);

        #endregion 公众号

        #region 小程序

        /// <summary>
        /// 小程序 临时登录凭证
        /// </summary>
        /// <param name="code"></param>
        /// <param name="acountId"></param>
        /// <returns></returns>
        Task<WeChatLoginInfo> LoginAsync(string code, int acountId);

        /// <summary>
        /// 小程序解密
        /// </summary>
        /// <param name="encryptedDataStr">包括敏感数据在内的完整用户信息的加密数据</param>
        /// <param name="key">用户的 session-key</param>
        /// <param name="iv">加密算法的初始向量</param>
        /// <returns></returns>

        T AESDecrypt<T>(string encryptedDataStr, string key, string iv);

        #endregion 小程序
    }
}
