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

        /// <summary>
        /// 小程序 临时登录凭证
        /// </summary>
        /// <param name="code"></param>
        /// <param name="acountId"></param>
        /// <returns></returns>
        public async Task<WeChatLoginInfo> LoginAsync(string code, int acountId)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("小程序未授权");
            }
            var wxConfig = _config.WechatAccount.FirstOrDefault(x => x.AcountId == acountId);
            if (wxConfig == null)
            {
                throw new Exception("小程序账号不存在");
            }
            var cacheKey = $"activity:wechat:{wxConfig.AppID}:{code}";
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
                if (result != null && !string.IsNullOrEmpty(result.session_key))
                {
                    //小程序登录Code有效期只有5分钟
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
                }
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
    }
}
