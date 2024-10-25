using Aliyun.Api.LogService.Infrastructure.Protocol;
using HappyShop.Comm;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Response;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions;
using Utility.Model;
using Utility.Model.Page;
using Utility.NetLog;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppConfig _config;

        /// <summary>
        ///
        /// </summary>
        public ApiClient(IOptionsMonitor<AppConfig> options,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _config = options.CurrentValue;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IndexPageResponse<JinNangListItem>> GetListPage(JinNangBackendPageRequest request)
        {
            try
            {
                var headers = new Dictionary<string, string>();
                var response = await InvokeAsync<IndexPageResponse<JinNangListItem>>(_config.TradeUrl + "/JinNang/BackData/Page?emapp-apikey=zhimakaimen", "POST", headers, request.ToJson());

                if (response?.Result?.Code != 0)
                {
                    Logger.WriteLog(Utility.Constants.LogLevel.Warning, "查询锦囊列表接口异常", new { request, response });
                    throw new Exception("查询锦囊列表接口异常");
                }
                return response.Detail;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Utility.Constants.LogLevel.Error, "查询锦囊列表接口异常", request, ex);
                throw;
            }
        }

        /// <summary>
        /// 获取交易动态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<RealTimeTradeItem>> GetTradeList(RealTimeTradeRequest request)
        {
            try
            {
                var headers = new Dictionary<string, string>();
                var response = await InvokeAsync<SlidePageResult<RealTimeTradeItem>>(_config.TradeUrl + "/JinNang/ConsoleData/TradePage?emapp-apikey=zhimakaimen", "POST", headers, request.ToJson());

                if (response?.Result?.Code != 0)
                {
                    Logger.WriteLog(Utility.Constants.LogLevel.Warning, "查询锦囊交易动态接口异常", new { request, response });
                    throw new Exception("查询锦囊交易动态接口异常");
                }
                var result = response.Detail.List;
                result.ForEach(item =>
                {
                    item.TotalPlStr = (item.TotalPl / (decimal)10000).ToString("p2").Replace(",", "");
                    item.EntrustPriceStr = (item.EntrustPrice / (decimal)10000.0).ToString("0.00");
                });
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Utility.Constants.LogLevel.Error, "查询锦囊交易动态接口异常", request, ex);
                throw;
            }
        }

        /// <summary>
        /// 获取持仓明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<HoldRepositoryItem>> GetHoldList(JinNangHoldRepositoryRequest request)
        {
            try
            {
                var headers = new Dictionary<string, string>();
                var response = await InvokeAsync<JinNangHoldRepositoryWrapper>(_config.TradeUrl + "/JinNang/ConsoleData/RepoList?emapp-apikey=zhimakaimen", "POST", headers, request.ToJson());

                if (response?.Result?.Code != 0)
                {
                    Logger.WriteLog(Utility.Constants.LogLevel.Warning, "获取锦囊持仓明细接口异常", new { request, response });
                    throw new Exception("获取锦囊持仓明细接口异常");
                }
                var result = response.Detail.Items;
                result.ForEach(item =>
                {
                    item.SecuScaleStr = (item.SecuScale / (decimal)10000).ToString("p2").Replace(",", "");
                });
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Utility.Constants.LogLevel.Error, "获取锦囊持仓明细接口异常", request, ex);
                throw;
            }
        }

        /// <summary>
        /// 通用回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public async Task<ApiData<T>> InvokeAsync<T>(string apiUrl, string method, Dictionary<string, string> headers, string requestContext)
        {
            using (var request = new HttpRequestMessage(new HttpMethod(method), apiUrl))
            {
                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                //POST请求可以传json数据
                if (string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(requestContext))
                {
                    request.Content = new StringContent(requestContext, Encoding.UTF8, "application/json");
                }
                using (var response = await _httpClientFactory.CreateClient().SendAsync(request))
                {
                    var responseContext = await response.Content.ReadAsStringAsync();
                    return responseContext.FromApiJson<ApiData<T>>();
                }
            }
        }
    }
}