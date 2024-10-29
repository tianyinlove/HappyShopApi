using HappyShop.Data;
using HappyShop.Domian;
using HappyShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class QYUserInfoService : IQYUserInfoService
    {
        private readonly IQYUserInfoData _userInfoData;
        private readonly IWeChatService _weChatService;

        /// <summary>
        ///
        /// </summary>
        public QYUserInfoService(IQYUserInfoData userInfoData,
            IWeChatService weChatService)
        {
            this._userInfoData = userInfoData;
            _weChatService = weChatService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> GetUserByUserIdAsync(string userId, int accountId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var result = await _userInfoData.GetUserByUserIdAsync(userId);
            if (result == null)
            {
                var weChatUser = await _weChatService.GetUserInfoAsync(userId, accountId);
                if (weChatUser != null && weChatUser.UserId == userId)
                {
                    result = new QYUserInfo
                    {
                        UserId = userId,
                        Name = weChatUser.Name,
                        Avatar = weChatUser.Avatar,
                        Gender = weChatUser.Gender,
                        Email = weChatUser.Email,
                        Mobile = weChatUser.Mobile,
                        Open_UserId = weChatUser.Open_UserId,
                        Status = weChatUser.Status,
                        Telephone = weChatUser.Telephone
                    };
                    result = await _userInfoData.SaveUpdateAsync(result);
                }
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> GetUserByIdAsync(string id)
        {
            return await _userInfoData.GetUserByIdAsync(id);
        }

        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<QYUserInfo> LoginAsync(string code, int accountId)
        {
            var user = await _weChatService.LoginAsync(code, accountId);
            if (user == null)
            {
                return null;
            }
            return await GetUserByUserIdAsync(user.UserId, accountId);
        }
    }
}