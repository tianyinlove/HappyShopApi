using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Documents;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Response;
using HappyShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using Utility.Model.Page;
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
        private readonly IStockMonitorService _stockMonitorService;

        /// <summary>
        ///
        /// </summary>
        public MyFollowController(IMyFollowData myFollowData,
            IStockMonitorService stockMonitorService,
            IApiClient apiClient)
        {
            _myFollowData = myFollowData;
            _stockMonitorService = stockMonitorService;
            this._apiClient = apiClient;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetListPage([FromBody] JinNangBackendPageRequest request)
        {
            var result = await _apiClient.GetListPage(request);
            return new ApiResult<IndexPageResponse<JinNangListItem>>(result);
        }

        /// <summary>
        /// 获取交易动态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTradeList([FromBody] RealTimeTradeRequest request)
        {
            var result = await _apiClient.GetTradeList(request);
            return new ApiResult<List<RealTimeTradeItem>>(result);
        }

        /// <summary>
        /// 获取持仓明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetHoldList([FromBody] JinNangHoldRepositoryRequest request)
        {
            var result = await _apiClient.GetHoldList(request);
            return new ApiResult<List<HoldRepositoryItem>>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string userName)
        {
            var result = await _myFollowData.GetMyFollows(userName);
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
        [HttpGet]
        public async Task<IActionResult> Follw(string userName, string stockPool, string stockCode = "", bool isFollow = true)
        {
            var result = await _myFollowData.SaveUpdate(userName, stockPool, stockCode, isFollow);
            return new ApiResult<bool>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task SendMessage()
        {
            await _stockMonitorService.SendMessageAsync();
        }
    }
}