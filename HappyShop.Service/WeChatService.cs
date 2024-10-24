﻿using HappyShop.Domian;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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
        private IHttpClientFactory _httpClientFactory;
        private IMemoryCache _memoryCache;
        private string getTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";
        private string loginUrl = "https://qyapi.weixin.qq.com/cgi-bin/miniprogram/jscode2session";
        private string sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send";
        private string userUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/get";
        private int _agentId = 1000002;
        private string _corpId = "ww1c5ca8f9af6164f4";
        private string _corpSecret = "0KpZS4ri3HuQOeiu0niga_peKXBp1--aTrviaTT8Z54";

        /// <summary>
        ///
        /// </summary>
        public WeChatService(IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken()
        {
            string cacheKey = "HappyShop:WeChat:Token";
            var result = _memoryCache.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(result))
            {
                var apiUrl = $"{getTokenUrl}?corpid={_corpId}&corpsecret={_corpSecret}";
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
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
        /// <returns></returns>
        public async Task<QYWechatLoginUser> LoginAsync(string code)
        {
            string cacheKey = $"HappyShop:WeChat:Login:{code}";
            var result = _memoryCache.Get<QYWechatLoginUser>(cacheKey);
            if (result == null)
            {
                var token = await GetToken();
                var apiUrl = $"{loginUrl}?access_token={token}&js_code={code}&grant_type=authorization_code";
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
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
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QYWechatUserInfo> GetUserInfoAsync(string userId)
        {
            string cacheKey = $"HappyShop:WeChat:UserInfo:{userId}";
            var result = _memoryCache.Get<QYWechatUserInfo>(cacheKey);
            if (result == null)
            {
                var token = await GetToken();
                var apiUrl = $"{userUrl}?access_token={token}&userid={userId}";
                var response = await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, apiUrl));
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
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
        /// <returns></returns>
        public async Task<bool> NoticeAsync(WechatMessageRequest request)
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
                var token = await GetToken();
                var apiUrl = $"{sendUrl}?access_token={token}";
                request.AgentId = _agentId;
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
                    Logger.WriteLog(LogLevel.Info, "发送消息", new
                    {
                        request,
                        data
                    });

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