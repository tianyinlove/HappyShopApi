using HappyShop.Comm;
using HappyShop.Domian;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    class OAuthService : IOAuthService
    {
        private AppConfig _config;
        private IMemoryCache _memoryCache;
        private HttpClient _httpClient = new HttpClient(new HttpClientHandler { UseProxy = false, Proxy = null });

        /// <summary>
        /// 
        /// </summary>
        public OAuthService(IOptionsMonitor<AppConfig> options,
            IMemoryCache memoryCache)
        {
            _config = options.CurrentValue;
            _memoryCache = memoryCache;
        }

        #region 公众号

        /// <summary>
        /// 微信获取code回调地址
        /// </summary>
        /// <param name="redirectUrl">redirectUrl</param>
        /// <param name="wxConfig"></param>
        /// <param name="isApp">是否是APP端</param>
        /// <returns></returns>
        public string GetWeChatCode(string redirectUrl, WechatAccount wxConfig, bool isApp = true)
        {
            if (wxConfig == null)
            {
                throw new Exception("账号不存在");
            }
            var url = "";
            if (isApp)
            {
                url = string.Format(_config.WechatConfig.WechatAppConnect, wxConfig.AppID, redirectUrl, "snsapi_userinfo");
            }
            else
            {
                url = string.Format(_config.WechatConfig.WechatPCConnect, wxConfig.AppID, redirectUrl, "snsapi_login");
            }

            return url;
        }

        /// <summary>
        /// 公众号 根据code获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wxConfig"></param>
        /// <returns></returns>
        public async Task<WeChatAccessTokenInfo> GetWeChatAccessTokenAsync(string code, WechatAccount wxConfig)
        {
            if (wxConfig == null)
            {
                throw new Exception("账号不存在");
            }
            string cacheKey = $"wechat:{wxConfig.AppID}:{code}";
            var result = _memoryCache.Get<WeChatAccessTokenInfo>(cacheKey);
            if (result != null)
            {
                return result;
            }
            string apiUrl = string.Format(_config.WechatConfig.WechatTokenUrl, wxConfig.AppID, wxConfig.AppSecret, code);
            using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
            {
                var responseMessage = await _httpClient.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                result = json.FromJson<WeChatAccessTokenInfo>();
                if (result != null && !string.IsNullOrEmpty(result.Access_Token))
                {
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromSeconds(result.Expires_In - 5));
                }
            }
            return result;
        }

        /// <summary>
        /// 公众号 根据accessToken和openId 获取用户信息
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public async Task<WeChatUserInfo> GetWeChatUserInfoAsync(string accessToken, string openId)
        {
            string apiUrl = string.Format(_config.WechatConfig.WechatUserUrl, accessToken, openId);
            using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
            {
                var responseMessage = await _httpClient.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                return json.FromJson<WeChatUserInfo>();
            }
        }

        /// <summary>
        /// 公众号 根据accessToken获取jsapi_ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<WechatTicketInfo> GetWeChatTicketAsync(string accessToken)
        {
            string cacheKey = $"activity:wechat:{accessToken}";
            var result = _memoryCache.Get<WechatTicketInfo>(cacheKey);
            if (result != null)
            {
                return result;
            }
            string apiUrl = string.Format(_config.WechatConfig.WechatTicketUrl, accessToken);

            using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
            {
                var responseMessage = await _httpClient.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                result = json.FromJson<WechatTicketInfo>();
                if (result != null && !string.IsNullOrEmpty(result.Ticket))
                {
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromSeconds(result.Expires_In - 5));
                }
            }
            return result;
        }

        #endregion 公众号

        #region 小程序

        /// <summary>
        /// 小程序 临时登录凭证
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wxConfig"></param>
        /// <returns></returns>
        public async Task<WeChatLoginInfo> LoginAsync(string code, WechatAccount wxConfig)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("小程序未授权");
            }
            if (wxConfig == null)
            {
                throw new Exception("小程序账号不存在");
            }
            var cacheKey = $"wechat:{wxConfig.AppID}:{code}";
            var result = _memoryCache.Get<WeChatLoginInfo>(cacheKey);
            if (result != null)
            {
                return result;
            }
            string apiUrl = string.Format(_config.WechatConfig.MiniSessionUrl, wxConfig.AppID, wxConfig.AppSecret, code);

            using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
            {
                var responseMessage = await _httpClient.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                result = json.FromJson<WeChatLoginInfo>();
                if (result == null || string.IsNullOrEmpty(result.session_key))
                {
                    throw new Exception("小程序授权异常");
                }
                //小程序登录Code有效期只有5分钟
                _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            }
            return result;
        }

        /// <summary>
        /// 小程序解密
        /// </summary>
        /// <param name="encryptedDataStr">包括敏感数据在内的完整用户信息的加密数据</param>
        /// <param name="key">用户的 session-key</param>
        /// <param name="iv">加密算法的初始向量</param>
        /// <returns></returns>

        public T AESDecrypt<T>(string encryptedDataStr, string key, string iv)
        {
            var rijalg = new RijndaelManaged();
            //-----------------    
            //设置 cipher 格式 AES-128-CBC    

            rijalg.KeySize = 128;

            rijalg.Padding = PaddingMode.PKCS7;
            rijalg.Mode = CipherMode.CBC;

            rijalg.Key = Convert.FromBase64String(key);
            rijalg.IV = Convert.FromBase64String(iv);

            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
            //解密    
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

            string result;

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        result = srDecrypt.ReadToEnd();
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                return result.FromJson<T>();
            }
            return default(T);
        }

        #endregion 小程序
    }
}
