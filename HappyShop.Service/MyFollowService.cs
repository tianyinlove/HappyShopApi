using HappyShop.Data;
using HappyShop.Documents;
using HappyShop.Model;
using MongoDB.Driver;
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
    internal class MyFollowService : IMyFollowService
    {
        private readonly IMyFollowData _myFollowData;
        private readonly IApiClient _apiClient;
        private readonly IQYUserInfoService _userInfoService;

        public MyFollowService(IMyFollowData myFollowData,
            IApiClient apiClient,
            IQYUserInfoService userInfoService)
        {
            _myFollowData = myFollowData;
            this._apiClient = apiClient;
            this._userInfoService = userInfoService;
        }

        /// <summary>
        /// 获取关注的股票
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <returns></returns>
        public async Task<List<MyFollowInfoDocument>> GetMyFollows(string token, string userId)
        {
            if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(userId))
            {
                var user = await _userInfoService.GetUserByIdAsync(token);
                if (user == null)
                {
                    return new List<MyFollowInfoDocument>();
                }
                userId = user.UserId;
            }
            return await _myFollowData.GetMyFollows(userId);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<bool> SaveUpdate(string token, string userId, string stockPool, string stockCode, bool isFollow, int accountId)
        {
            QYUserInfo user;
            //查询用户是否存在
            if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(userId))
            {
                user = await _userInfoService.GetUserByIdAsync(token);
                userId = user?.UserId;
            }
            else
            {
                user = await _userInfoService.GetUserByUserIdAsync(userId, accountId);
            }
            if (user == null)
            {
                return false;
            }
            //查询股票池是否存在
            var stockPoolList = await _apiClient.GetListPage(new Request.JinNangBackendPageRequest { JinNangName = stockPool });
            if (stockPoolList == null || stockPoolList.Total <= 0)
            {
                return false;
            }

            return await _myFollowData.SaveUpdate(userId, stockPool, stockCode, isFollow);
        }
    }
}