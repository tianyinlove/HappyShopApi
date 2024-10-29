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
        private readonly IMyFollowService _myFollowService;
        private readonly IApiClient _apiClient;
        private readonly IStockMonitorService _stockMonitorService;

        /// <summary>
        ///
        /// </summary>
        public MyFollowController(IMyFollowService myFollowService,
            IStockMonitorService stockMonitorService,
            IApiClient apiClient)
        {
            _myFollowService = myFollowService;
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
        /// <param name="userId">用户在企业内的UserID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyFollows([FromQuery] string userId)
        {
            var result = await _myFollowService.GetMyFollows(userId);
            return new ApiResult<List<MyFollowInfoDocument>>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId">用户在企业内的UserID</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Follw([FromQuery] string userId, string stockPool = "", string stockCode = "", bool isFollow = true, int accountId = 4)
        {
            var result = await _myFollowService.SaveUpdate(userId, stockPool, stockCode, isFollow, accountId);
            return new ApiResult<bool>(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromQuery] int accountId = 4)
        {
            await _stockMonitorService.SendMessageAsync(accountId);
            return new ApiResult<bool>(true);
        }
    }
}