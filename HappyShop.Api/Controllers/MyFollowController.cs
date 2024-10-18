using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Documents;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.NetCore;

namespace HappyShop.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class MyFollowController : ControllerBase
    {
        private readonly IMyFollowData _myFollowData;
        private readonly IApiClient _apiClient;

        /// <summary>
        ///
        /// </summary>
        public MyFollowController(IMyFollowData myFollowData,
            IApiClient apiClient)
        {
            _myFollowData = myFollowData;
            this._apiClient = apiClient;
        }

        /// <summary>
        ///当前持仓数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<HoldRepositoryItem>> GetStockListById(int prodid)
        {
            return await _apiClient.GetStockListByIdAsync(prodid);
        }

        /// <summary>
        ///当日交易记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<StockTradeInfo>> GetStockTradeListByName(string name)
        {
            return await _apiClient.GetStockTradeListByNameAsync(name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string uid)
        {
            var result = await _myFollowData.GetMyFollows(uid);
            return new ApiResult<List<MyFollowInfoDocument>>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <returns></returns>
        public async Task<IActionResult> Follw(string userName, string stockPool, string stockCode = "", bool isFollow = true)
        {
            var result = await _myFollowData.SaveUpdate(userName, stockPool, stockCode, isFollow);
            return new ApiResult<bool>(result);
        }
    }
}