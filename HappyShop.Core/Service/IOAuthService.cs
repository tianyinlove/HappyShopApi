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
    }
}
