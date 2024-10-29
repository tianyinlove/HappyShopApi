using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Domian;
using HappyShop.Model;
using HappyShop.Repositories;
using HappyShop.Request;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Extensions;
using Utility.NetLog;
using Utility.Redis;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class StockMonitorService : IStockMonitorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IMyRedisClient _redisClient;
        private readonly IWeChatService _weChatService;
        private readonly AppConfig _appSettings;
        private readonly IApiClient _apiClient;
        private readonly IMyFollowData _myFollowData;

        /// <summary>
        ///
        /// </summary>
        public StockMonitorService(IMemoryCache memoryCache,
            IMyRedisClient redisClient,
            IWeChatService weChatService,
            IOptionsMonitor<AppConfig> options,
            IApiClient apiClient,
            IMyFollowData myFollowData,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            this._redisClient = redisClient;
            _weChatService = weChatService;
            _appSettings = options.CurrentValue;
            _apiClient = apiClient;
            this._myFollowData = myFollowData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task SendMessageAsync(int accountId)
        {
            Logger.WriteLog(LogLevel.Info, "开始执行服务");

            var w = DateTime.Now.DayOfWeek;
            var h = DateTime.Now.Hour;
            if (w == DayOfWeek.Saturday || w == DayOfWeek.Sunday || h < _appSettings.StartHour || h > _appSettings.EndHour)
            {
                return;
            }

            var userData = await _myFollowData.GetMyFollows("");
            if (userData == null || userData.Count <= 0)
            {
                return;
            }

            var stockPoolList = userData.Select(x => x.StockPool).Distinct().ToList() ?? new List<string>();

            foreach (var poolName in stockPoolList)
            {
                try
                {
                    var stockData = await GetStockTradeListByNameAsync(poolName);
                    if (stockData != null && stockData.Count > 0)
                    {
                        var cacheKey = $"stocktrade:time:{poolName.Md5()}";
                        var time = _redisClient.Get<DateTime>(cacheKey);
                        if (time < DateTime.Today)
                        {
                            time = DateTime.Today;
                        }
                        var list = stockData.Where(x => x.TradeTime > time).ToList();
                        if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var message = $"{item.TradeTime.ToString("yyyy-MM-dd HH:mm:ss")}\n{item.TradeTypeName}：{item.SecuName}({item.SecuCode})\n委托价：{item.EntrustPriceStr}元({item.EntrustAmt}股)，撤单{item.CancleAmt}股\n成交价：{item.DealPriceStr}元({item.DealNumber}股)\n状态：{item.StatusMsg}\n成交仓位：{item.DealPosition};\n\n";
                                var toUsers = userData.Where(x => (x.StockPool == poolName && string.IsNullOrEmpty(x.StockCode)) || (x.StockPool == poolName && x.StockCode == item.SecuCode) || (string.IsNullOrEmpty(x.StockPool) && x.StockCode == item.SecuCode))
                                    .Select(x => x.UserName)
                                    .Distinct()
                                    .ToList();
                                if (toUsers != null && toUsers.Count > 0)
                                {
                                    //每次读取1000条
                                    var toUsersCount = toUsers.Count;
                                    for (int i = 0; i < toUsersCount; i += 1000)
                                    {
                                        var toUserNames = toUsers.Skip(i).Take(1000).ToList();
                                        if (toUserNames != null && toUserNames.Count > 0)
                                        {
                                            var request = new WechatMessageRequest
                                            {
                                                ToUser = string.Join("|", toUserNames),
                                                MsgType = NoticeType.text.ToString(),
                                                Text = new NoticeText { Content = $"{poolName}\n{message}" }
                                            };

                                            await _weChatService.NoticeAsync(request, accountId);
                                        }
                                    }
                                }
                            }
                        }
                        time = stockData.Max(x => x.TradeTime);
                        _redisClient.Set(cacheKey, time, TimeSpan.FromDays(1));
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevel.Error, "读取好股数据异常", poolName, ex);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<List<RealTimeTradeItem>> GetStockTradeListByNameAsync(string name)
        {
            var data = await _apiClient.GetListPage(new JinNangBackendPageRequest { JinNangName = name });
            if (data == null || data.List == null || data.List.Count == 0)
            {
                return new List<RealTimeTradeItem>();
            }
            var result = await _apiClient.GetTradeList(new RealTimeTradeRequest { JinNangId = data.List[0].JinNangId, PageSize = 200 });
            if (result != null && result.Count > 0)
            {
                var cacheKey = $"stocktrade:data:{name.Md5()}";
                var oldData = _memoryCache.Get<List<RealTimeTradeItem>>(cacheKey) ?? new List<RealTimeTradeItem>();
                if (oldData.Count > 0)
                {
                    result = result.Where(x =>
                        !oldData.Exists(o =>
                        o.TradeType == x.TradeType &&
                        o.TradeTime == x.TradeTime &&
                        o.SecuName == x.SecuName &&
                        o.SecuCode == x.SecuCode &&
                        o.CancleAmt == x.CancleAmt &&
                        o.DealAmount == x.DealAmount &&
                        o.DealPosition == x.DealPosition &&
                        o.DealPrice == x.DealPrice &&
                        o.DealNumber == x.DealNumber &&
                        o.DealTime == x.DealTime &&
                        o.EntrustTime == x.EntrustTime &&
                        o.EntrustAmt == x.EntrustAmt &&
                        o.EntrustPrice == x.EntrustPrice))
                        .ToList() ?? new List<RealTimeTradeItem>();
                }

                oldData.AddRange(result);
                _memoryCache.Set(cacheKey, oldData, TimeSpan.FromMinutes(10));
            }
            return result;
        }
    }
}