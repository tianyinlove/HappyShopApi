using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HappyShop.Comm;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.Extensions;
using Utility.Model;
using Utility.NetCore;

namespace HappyShop.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserInfoService _userInfoService;
        private readonly AppConfig _config;
        private readonly IOAuthService _oauthService;

        /// <summary>
        /// 
        /// </summary>
        public UserController(IHttpContextAccessor httpContext,
            IOptionsMonitor<AppConfig> options,
            IOAuthService oauthService,
            IUserInfoService userInfoService)
        {
            _httpContext = httpContext;
            _userInfoService = userInfoService;
            _config = options.CurrentValue;
            _oauthService = oauthService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<ApiResult<UserInfo>> Login([FromBody]LoginRequest request)
        {
            var user = await _userInfoService.LoginAsync(request);
            return new ApiResult<UserInfo>(user);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<UserInfo>> Register([FromBody]LoginRequest request)
        {
            var user = await _userInfoService.RegisterAsync(request);
            return new ApiResult<UserInfo>(user);
        }

        #region 微信

        /// <summary>
        /// 被分享者首次授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task WeChat(int acountId = 2, string code = "")
        {
            var query = _httpContext.HttpContext.Request.QueryString.Value;
            try
            {
                var wxConfig = _config.WechatAccount.FirstOrDefault(x => x.AcountId == acountId);
                if (string.IsNullOrEmpty(code))
                {
                    //首次握手               
                    string redirectUrl = $"{ wxConfig.RedirectUrl}{query}";
                    _httpContext.HttpContext.Response.Redirect(_oauthService.GetWeChatCode(WebUtility.UrlEncode(redirectUrl), acountId, true), true);
                }
                else
                {
                    var accessToken = await _oauthService.GetWeChatAccessTokenAsync(code, acountId);
                    if (accessToken == null)
                    {
                        throw new ApiException(-1, "code无效");
                    }
                    var ticketInfo = await _oauthService.GetWeChatTicketAsync(accessToken.Access_Token);
                    if (ticketInfo == null)
                    {
                        throw new ApiException(-1, "授权失败");
                    }
                    var urlQuery = $"jsapi_ticket={ticketInfo.Ticket}&noncestr={Guid.NewGuid().ToString().Replace("-", "")}&timestamp={DateTime.Now.ValueOf()}";
                    //加密
                    var signature = $"{urlQuery}&url={wxConfig.SuccessUrl}".Sha1();
                    string redirectUrl = $"{ wxConfig.SuccessUrl}{query}&{urlQuery}&signature={signature}";
                    _httpContext.HttpContext.Response.Redirect(redirectUrl);
                }
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ApiException(-1, "服务器异常");
            }
        }

        #endregion 微信
    }
}