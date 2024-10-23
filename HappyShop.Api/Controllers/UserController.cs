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
using Microsoft.Extensions.Options;
using Utility.Extensions;
using Utility.Model;
using Utility.NetCore;
using HappyShop.Domian;
using Utility.NetLog;
using System.IO;

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
        private readonly IOptionsMonitor<AppConfig> _options;
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
            _options = options;
            _oauthService = oauthService;
        }

        /// <summary>
        ///
        /// </summary>
        private AppConfig Appconfig
        {
            get { return _options.CurrentValue; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return new ApiResult<AppConfig>(Appconfig);
        }

        /// <summary>
        /// 微信/用户名密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] WechatRequest request)
        {
            var result = await _userInfoService.LoginAsync(request);
            if (result != null && !string.IsNullOrEmpty(result.PhoneNumber))
            {
                result.PhoneNumber = result.PhoneNumber.MaskPhone();
            }
            return new ApiResult<UserInfo>(result);
        }

        /// <summary>
        /// 注册/修改用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BindUser([FromBody] UserReuqest request)
        {
            var result = await _userInfoService.SaveUpdateAsync(request);
            if (result != null && !string.IsNullOrEmpty(result.PhoneNumber))
            {
                result.PhoneNumber = result.PhoneNumber.MaskPhone();
            }
            return new ApiResult<UserInfo>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="acountId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetJsApiSign(int acountId, string url)
        {
            var result = await _userInfoService.GetJsApiSign(acountId, url);
            return new ApiResult<WechatJSTicket>(result);
        }

        #region 微信

        /// <summary>
        /// 被分享者首次授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool CheckSignature(int acountId = 2)
        {
            var query = _httpContext.HttpContext.Request.QueryString.Value;
            Logger.WriteLog(Utility.Constants.LogLevel.Debug, $"微信数据 {query}");
            return true;
        }

        /// <summary>
        /// 被分享者首次授权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void WeChat(int acountId = 2)
        {
            var query = _httpContext.HttpContext.Request.QueryString.Value;
            var wxConfig = Appconfig.WechatAccount.FirstOrDefault(x => x.AcountId == acountId);
            //首次握手
            string redirectUrl = $"{wxConfig.RedirectUrl}{query}";
            _httpContext.HttpContext.Response.Redirect(_oauthService.GetWeChatCode(WebUtility.UrlEncode(redirectUrl), wxConfig, true), true);
        }

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="acountId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> WeChatUser(int acountId, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ApiException(-1, "code不能为空");
            }
            var wxConfig = Appconfig.WechatAccount.FirstOrDefault(x => x.AcountId == acountId);
            var accessToken = await _oauthService.GetWeChatAccessTokenAsync(wxConfig, code);
            if (accessToken == null)
            {
                throw new ApiException(-1, "code无效");
            }

            var result = await _oauthService.GetWeChatUserInfoAsync(accessToken.Access_Token, accessToken.OpenId);
            return new ApiResult<WeChatUserInfo>(result);
        }

        #endregion 微信
    }
}