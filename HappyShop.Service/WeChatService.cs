using HappyShop.Comm;
using HappyShop.Domian;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Extensions;
using Utility.NetLog;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class WeChatService : IWeChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptionsMonitor<AppConfig> _options;

        /// <summary>
        ///
        /// </summary>
        public WeChatService(IMemoryCache memoryCache,
            IOptionsMonitor<AppConfig> options,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            this._options = options;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private async Task<string> GetToken(int accountId)
        {
            string cacheKey = "HappyShop:WeChat:Token";
            var result = _memoryCache.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(result))
            {
                var account = _options.CurrentValue.WechatAccount.FirstOrDefault(x => x.AccountId == accountId);
                var apiUrl = string.Format(_options.CurrentValue.QYWechatConfig.WechatTokenUrl, account.AppID, account.AppSecret);
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    Logger.WriteLog(LogLevel.Debug, "企业微信获取Token异常", new { apiUrl, data });
                    var responseData = data.FromJson<WechatMessageResponse>();
                    if (responseData.ErrCode != 0)
                    {
                        throw new Exception(responseData.ErrMsg);
                    }
                    result = responseData.Access_Token;
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromSeconds(responseData.Expires_In - 10));
                }
            }
            return result;
        }

        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<QYWechatLoginUser> LoginAsync(string code, int accountId)
        {
            string cacheKey = $"HappyShop:WeChat:Login:{code}";
            var result = _memoryCache.Get<QYWechatLoginUser>(cacheKey);
            if (result == null)
            {
                var token = await GetToken(accountId);
                var apiUrl = string.Format(_options.CurrentValue.QYWechatConfig.WechatTicketUrl, token, code);
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    Logger.WriteLog(LogLevel.Debug, "企业微信登录异常", new { apiUrl, data });
                    var responseData = data.FromJson<QYWechatLoginUser>();
                    if (responseData.ErrCode != 0)
                    {
                        throw new Exception(responseData.ErrMsg);
                    }
                    result = responseData;
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QYWechatUserInfo> GetUserInfoAsync(string userId, int accountId)
        {
            string cacheKey = $"HappyShop:WeChat:UserInfo:{userId}";
            var result = _memoryCache.Get<QYWechatUserInfo>(cacheKey);
            if (result == null)
            {
                var token = await GetToken(accountId);
                var apiUrl = string.Format(_options.CurrentValue.QYWechatConfig.WechatUserUrl, token, userId);
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    Logger.WriteLog(LogLevel.Debug, "企业微信获取用户信息异常", new { apiUrl, data });
                    var responseData = data.FromJson<QYWechatUserInfo>();
                    if (responseData.ErrCode != 0)
                    {
                        throw new Exception(responseData.ErrMsg);
                    }
                    result = responseData;
                    _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }
            }
            return result;
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="request">通知对象</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<bool> NoticeAsync(WechatMessageRequest request, int accountId)
        {
            if (string.IsNullOrEmpty(request.ToUser) && string.IsNullOrEmpty(request.ToTag) && string.IsNullOrEmpty(request.ToParty))
            {
                throw new Exception("通知用户不能为空");
            }
            if (request.News == null && request.Image == null && request.File == null && request.Text == null && request.TextCard == null && request.Video == null && request.Voice == null)
            {
                throw new Exception("通知内容不能为空");
            }
            try
            {
                var account = _options.CurrentValue.WechatAccount.FirstOrDefault(x => x.AccountId == accountId);
                var token = await GetToken(accountId);
                var apiUrl = string.Format(_options.CurrentValue.QYWechatConfig.WechatSendUrl, token);
                request.AgentId = Convert.ToInt32(account.AgentId);
                var jsonData = request.ToJson(NullValueHandling.Ignore);
                var requestData = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                };
                var response = await _httpClientFactory.CreateClient().SendAsync(requestData);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    Logger.WriteLog(LogLevel.Debug, "发送消息", new { request, data });
                    var responseData = data.FromJson<WechatMessageResponse>();
                    if (responseData.ErrCode != 0)
                    {
                        throw new Exception(responseData.ErrMsg);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevel.Error, "发送微信消息通知异常", request, ex);
                return false;
            }
        }
    }
}