using HappyShop.Comm;
using HappyShop.Data;
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
using HappyShop.Entity;
using HappyShop.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HappyShop.Domian;
using System.Collections.Generic;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class UserInfoService : IUserInfoService
    {
        private IUserInfoData _userInfoData;
        private IOAuthService _oathService;
        private IApiClient _apiClient;
        private AppConfig _config;

        /// <summary>
        ///
        /// </summary>
        public UserInfoService(IOptionsMonitor<AppConfig> options,
            IOAuthService oAuthService,
            IApiClient apiClient,
            IUserInfoData userInfoData)
        {
            _userInfoData = userInfoData;
            _oathService = oAuthService;
            _apiClient = apiClient;
            _config = options.CurrentValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> GetStockTradeByNameAsync(string name)
        {
            var result = "今日无交易";
            try
            {
                var data = await _apiClient.GetListPage(new JinNangBackendPageRequest { JinNangName = name });
                if (data == null || data.List == null || data.List.Count == 0)
                {
                    return result;
                }
                var list = await _apiClient.GetTradeList(new RealTimeTradeRequest { JinNangId = data.List[0].JinNangId, PageSize = 200 });
                if (list != null && list.Count > 0)
                {
                    result = "";
                    list.ForEach(item =>
                    {
                        result += $"{item.TradeTime}\n{item.TradeTypeName}：{item.SecuName}({item.StockCode})\n委托价：{item.EntrustPrice}元({item.EntrustAmt}股)，撤单{item.CancleAmt}股\n成交价：{item.DealPrice}元({item.DealAmount}股)\n状态：{item.StatusMsg}\n成交仓位：{item.DealPosition};\n\n";
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevel.Error, "读取好股交易数据异常", name, ex);
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prodid"></param>
        /// <returns></returns>
        public async Task<string> GetStockMessageByIdAsync(int prodid)
        {
            var result = "当前空仓";
            try
            {
                var list = await _apiClient.GetHoldList(new JinNangHoldRepositoryRequest { JinNangId = prodid, PageSize = 200 });
                if (list != null && list.Count > 0)
                {
                    result = "";
                    list.ForEach(item =>
                    {
                        result += $"{item.SecuritiesName}({item.SecuritiesCode}),持仓：{item.SecuAmuntStr}股({item.SecuScaleStr}),成本：{item.DilucostPriceStr},盈亏：{item.GainLossScale};\n\n";
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevel.Error, "读取好股持仓数据异常", prodid, ex);
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserInfo> LoginAsync(WechatRequest request)
        {
            var result = new UserInfoEntity();
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                var wxConfig = _config.WechatAccount.FirstOrDefault(x => x.AcountId == request.AcountId);
                //微信登录
                var loginInfo = await _oathService.LoginAsync(request.Code, wxConfig);
                result = await _userInfoData.GetUserByAccount(loginInfo.unionid ?? loginInfo.openid);
                if (result == null)
                {
                    result = new UserInfoEntity
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
                    var wx_user = _oathService.AESDecrypt<Domian.MiniUserPhone>(request.EncryptedData, loginInfo.session_key, request.Iv);
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
                result = await _userInfoData.GetUserByAccount(request.PhoneNumber);
                if (result == null)
                {
                    throw new ApiException(-1, "账号不存在，请先注册");
                }
                if (string.IsNullOrEmpty(request.PassWord) || request.PassWord != result.PassWord)
                {
                    throw new ApiException(-1, "登录密码错误");
                }
            }
            return result.Convert<UserInfoEntity, UserInfo>();
        }

        /// <summary>
        /// 修改或者注册用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserInfo> SaveUpdateAsync(UserReuqest request)
        {
            var result = new UserInfoEntity();
            if (!string.IsNullOrEmpty(request.Id))
            {
                result = await _userInfoData.GetUserById(request.Id);
            }
            if (result == null)
            {
                result = new UserInfoEntity();
            }
            //新账号需要判断手机号和密码是否正常
            if (string.IsNullOrEmpty(result.Id))
            {
                if (string.IsNullOrEmpty(request.PhoneNumber))
                {
                    throw new ApiException(-1, "手机号不能为空");
                }

                if (!request.PhoneNumber.IsPhone())
                {
                    throw new ApiException(-1, "手机号不正确");
                }

                if (string.IsNullOrEmpty(request.NickName))
                {
                    throw new ApiException(-1, "昵称不能为空");
                }

                if (string.IsNullOrEmpty(request.PassWord))
                {
                    throw new ApiException(-1, "密码不能为空");
                }
            }
            //修改头像
            if (!string.IsNullOrEmpty(request.HeadImg))
            {
                result.HeadImg = request.HeadImg;
            }
            //修改昵称
            if (!string.IsNullOrEmpty(request.NickName))
            {
                result.NickName = request.NickName;
            }
            //修改密码时间判断密码长度
            if (!string.IsNullOrEmpty(request.PassWord))
            {
                if (request.PassWord.Length < 6)
                {
                    throw new ApiException(-1, "密码长度不能小于6位");
                }
                result.PassWord = request.PassWord;
            }
            //修改手机时判断手机是否被绑定
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                if (request.PhoneNumber.IndexOf("*") < 0)
                {
                    var old = await _userInfoData.GetUserByAccount(request.PhoneNumber);
                    if (old != null && old.Id.ToString() != request.Id)
                    {
                        throw new ApiException(-1, "手机号已被注册或已被其它账号绑定");
                    }
                    result.PhoneNumber = request.PhoneNumber;
                }
            }

            result = await _userInfoData.SaveUpdate(result);
            return result.Convert<UserInfoEntity, UserInfo>();
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