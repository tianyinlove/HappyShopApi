using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.Model;
using Utility.Model.Page;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IndexPageResponse<JinNangListItem>> GetListPage(JinNangBackendPageRequest request);

        /// <summary>
        /// 获取交易动态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<RealTimeTradeItem>> GetTradeList(RealTimeTradeRequest request);

        /// <summary>
        /// 获取持仓明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<HoldRepositoryItem>> GetHoldList(JinNangHoldRepositoryRequest request);
    }
}