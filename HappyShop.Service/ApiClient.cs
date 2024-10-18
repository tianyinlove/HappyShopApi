using HappyShop.Comm;
using HappyShop.Domian;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions;
using Utility.Model;
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
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<HoldRepositoryItem>> GetStockListByIdAsync(int prodid)
        {
            var result = new List<HoldRepositoryItem>();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("X-Protocol-Id", "9300");
                headers.Add("X-Request-Id", Guid.NewGuid().ToString("N"));
                var response = await InvokeAsync<PflQrySecuShareResponse>(_config.TradeUrl, "POST", headers, (new
                {
                    token = "",
                    prodid = prodid,
                    market = 0,
                    pos = 0,
                    req = 1000,
                    stockcode = 0
                }).ToJson());

                if (response?.Result?.Code != 0)
                {
                    Logger.WriteLog(Utility.Constants.LogLevel.Warning, "读取持仓信息业务接口异常", new { prodid, response });
                    throw new Exception("读取持仓信息业务接口异常");
                }
                if (response?.Detail?.Secushare != null)
                {
                    result = response.Detail?.Secushare.Select(a => new HoldRepositoryItem
                    {
                        BusMsg = a.Busimsg,
                        DilucostPrice = a.Dilucostprice,
                        GainLossScale = a.Unplscale switch
                        {
                            -10000 => "无成本",
                            0 => "0",
                            100_0000 => "五倍+",
                            _ => (a.Unplscale / (double)10000).ToString("p2").Replace(",", "")
                        },
                        TotalGailLoss = a.Totalpl * 10,
                        NewPrice = (int)a.Newprice,
                        SecuCost = (int)a.Secucost,
                        SecuritiesCode = a.Secucode,
                        SecuritiesName = a.Secuname,
                        SecuScale = ((int)a.Secuscale),
                        Idx = (int)a.Idx,
                        IndustryName = a.Indusname,
                        SecuAmount = (int)a.Secuamount,
                        UsableAmount = (int)a.Usableamount,
                        IndustryId = (int)a.Indusid,
                        MarketValue = ((long)a.Marketvalue) * 10,
                        DilucostPriceStr = (a.Dilucostprice / 10000.0).ToString("0.000"),
                        NewPriceStr = (a.Newprice / 10000.0).ToString("0.000"),
                        SecuAmuntStr = a.Secuamount.ToString("0.00"),
                        UsableAmountStr = a.Usableamount.ToString("0.00"),
                        SecuScaleStr = (a.Secuscale / (double)10000).ToString("p2").Replace(",", "")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Utility.Constants.LogLevel.Error, "读取持仓信息异常", prodid, ex);
                throw;
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<StockTradeInfo>> GetStockTradeListByNameAsync(string name)
        {
            var result = new List<StockTradeInfo>();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("X-Protocol-Id", "9400");
                headers.Add("X-Request-Id", Guid.NewGuid().ToString("N"));
                var response = await InvokeAsync<StockQryEntrustResponse>(_config.TradeUrl, "POST", headers, (new
                {
                    token = "",
                    prodid = 0,
                    market = 0,
                    secucode = "",
                    startdate = Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")),
                    enddate = Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")),
                    entrustNo = 0,
                    filterDeal = 0,
                    filterBS = 0,
                    pos = 0,
                    req = 20,
                    stockcode = 0,
                    zoneid = 2801,
                    prodtitle = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(name)),
                    authorname = ""
                }).ToJson());

                if (response?.Result?.Code != 0)
                {
                    Logger.WriteLog(Utility.Constants.LogLevel.Warning, "读取交易信息业务接口异常", new { name, response });
                    throw new Exception("读取交易信息业务接口异常");
                }
                if (response?.Detail?.entrust != null)
                {
                    result = response.Detail?.entrust.Select(a => new StockTradeInfo
                    {
                        StockCode = a.Secucode,
                        Secuname = a.Secuname,
                        TradeTime = string.IsNullOrEmpty(a.Dealtime) ? DateTime.Now : Convert.ToDateTime(a.Dealtime),
                        Busimsg = a.Busimsg,
                        DealAmount = a.Dealamt,
                        Entrustamt = a.Entrustamt,
                        Cancelamt = a.Cancelamt,
                        EntrustPrice = (decimal)(a.Entrustprice / 10000.0),
                        DealPrice = (decimal)(a.Dealprice / 10000.0),
                        DealPosition = (a.Ownratio / (double)10000).ToString("p2").Replace(",", ""),
                        Stkpospre = (a.Stkpospre / (double)10000).ToString("p2").Replace(",", ""),
                        Stkposdst = (a.Stkposdst / (double)10000).ToString("p2").Replace(",", "")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Utility.Constants.LogLevel.Error, "读取交易信息异常", name, ex);
                throw;
            }
            return result;
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
                    return responseContext.FromJson<ApiData<T>>();
                }
            }
        }
    }
}