using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Documents;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Request;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    class UserInfoService : IUserInfoService
    {
        private IUserInfoData _userInfoData;
        private IOAuthService _oathService;

        /// <summary>
        /// 
        /// </summary>
        public UserInfoService(IOAuthService oAuthService,
            IUserInfoData userInfoData)
        {
            _userInfoData = userInfoData;
            _oathService = oAuthService;
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
                //微信登录
                var loginInfo = await _oathService.LoginAsync(request.Code, request.AcountId);
                result = await _userInfoData.GetUserInfo(loginInfo.unionid);
                if (result == null)
                {
                    result = new UserInfoDocument
                    {
                        Unionid = loginInfo.unionid,
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
                    if (wx_user != null)
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
                    throw new Exception("账号不存在，请先注册");
                }
                if (string.IsNullOrEmpty(request.PassWord) || request.PassWord != result.PassWord)
                {
                    throw new Exception("登录密码错误");
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
                throw new Exception("注册账号不能为空");
            }
            if (string.IsNullOrEmpty(request.PassWord))
            {
                throw new Exception("密码不能为空");
            }
            if (request.PassWord.Length < 6)
            {
                throw new Exception("密码不能少于6位");
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                if (await _userInfoData.GetUserInfo(request.PhoneNumber) != null)
                {
                    throw new Exception("手机号已被注册或已被其它账号绑定");
                }
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                if (await _userInfoData.GetUserInfo(request.Email) != null)
                {
                    throw new Exception("邮箱已被注册或已被其它账号绑定");
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
    }
}
