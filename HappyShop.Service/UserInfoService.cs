using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Documents;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Request;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utility.Extensions;
using Utility.NetLog;
using Utility.Model;
using Utility.Constants;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    class UserInfoService : IUserInfoService
    {
        private IUserInfoData _userInfoData;
        private IOAuthService _oathService;
        private AppConfig _config;

        /// <summary>
        /// 
        /// </summary>
        public UserInfoService(IOptionsMonitor<AppConfig> options,
            IOAuthService oAuthService,
            IUserInfoData userInfoData)
        {
            _userInfoData = userInfoData;
            _oathService = oAuthService;
            _config = options.CurrentValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserInfo> LoginAsync(LoginRequest request)
        {
            var accountName = request.PhoneNumber;
            if (string.IsNullOrEmpty(accountName))
            {
                accountName = request.Email;
            }
            var result = new UserInfoDocument();
            if (string.IsNullOrEmpty(accountName))
            {
                var wxConfig = _config.WechatAccount.FirstOrDefault(x => x.AcountId == request.AcountId);
                //微信登录
                var loginInfo = await _oathService.LoginAsync(request.Code, wxConfig);
                result = await _userInfoData.GetUserInfo(loginInfo.unionid ?? loginInfo.openid);
                if (result == null)
                {
                    result = new UserInfoDocument
                    {
                        UnionId = loginInfo.unionid,
                        OpenId = loginInfo.openid,
                        CreateTime = DateTime.Now,
                        HeadImg = request.HeadImg,
                        NickName = request.NickName,
                        UpdateTime = DateTime.Now
                    };
                }

                //如果手机号为空
                if (string.IsNullOrEmpty(result.PhoneNumber) && !string.IsNullOrEmpty(request.EncryptedData) && !string.IsNullOrEmpty(request.Iv))
                {
                    //数据解密,序列化获取手机号码
                    var wx_user = _oathService.AESDecrypt<MiniUserPhone>(request.EncryptedData, loginInfo.session_key, request.Iv);
                    if (wx_user != null && !string.IsNullOrEmpty(wx_user.PhoneNumber))
                    {
                        result.PhoneNumber = wx_user.PhoneNumber;
                    }
                }
                result = await _userInfoData.SaveUpdate(result);
            }
            else
            {
                //账号密码登录
                result = await _userInfoData.GetUserInfo(accountName);
                if (result == null)
                {
                    throw new ApiException(-1, "账号不存在，请先注册");
                }
                if (string.IsNullOrEmpty(request.PassWord) || request.PassWord != result.PassWord)
                {
                    throw new ApiException(-1, "登录密码错误");
                }
            }
            return result.Convert<UserInfoDocument, UserInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserInfo> RegisterAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) && string.IsNullOrEmpty(request.Email))
            {
                throw new ApiException(-1, "注册账号不能为空");
            }
            if (string.IsNullOrEmpty(request.PassWord))
            {
                throw new ApiException(-1, "密码不能为空");
            }
            if (request.PassWord.Length < 6)
            {
                throw new ApiException(-1, "密码不能少于6位");
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                if (await _userInfoData.GetUserInfo(request.PhoneNumber) != null)
                {
                    throw new ApiException(-1, "手机号已被注册或已被其它账号绑定");
                }
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                if (await _userInfoData.GetUserInfo(request.Email) != null)
                {
                    throw new ApiException(-1, "邮箱已被注册或已被其它账号绑定");
                }
            }

            var result = new UserInfoDocument
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                HeadImg = request.HeadImg,
                NickName = request.NickName,
                PassWord = request.PassWord
            };
            result = await _userInfoData.SaveUpdate(result);
            return result.Convert<UserInfoDocument, UserInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acountId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<WechatJSTicket> GetJsApiSign(int acountId, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ApiException(-1, "参数异常");
            }
            var wxConfig = _config.WechatAccount.FirstOrDefault(x => x.AcountId == acountId);
            var accessToken = await _oathService.GetWeChatAccessTokenAsync(wxConfig);
            if (accessToken == null || string.IsNullOrEmpty(accessToken.Access_Token))
            {
                throw new ApiException(-1, "获取JSAPITock失败");
            }
            var ticketInfo = await _oathService.GetWeChatTicketAsync(accessToken.Access_Token);
            if (ticketInfo == null || string.IsNullOrEmpty(ticketInfo.Ticket))
            {
                throw new ApiException(-1, "授权失败");
            }
            var result = new WechatJSTicket
            {
                NonceStr = Guid.NewGuid().ToString().Replace("-", ""),
                Timestamp = DateTime.Now.ValueOf()
            };
            url = HttpUtility.UrlDecode(url);
            var urlQuery = $"jsapi_ticket={ticketInfo.Ticket}&noncestr={result.NonceStr}&timestamp={result.Timestamp}&url={url}";
            //加密
            result.Signature = urlQuery.Sha1();

            Logger.WriteLog(LogLevel.Debug, $"微信加密数据 {urlQuery} {result.ToJson()}");

            return result;
        }
    }
}
