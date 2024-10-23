﻿using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Domian;
using HappyShop.Model;
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

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class StockMonitorService : IStockMonitorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IWeChatService _weChatService;
        private readonly AppConfig _appSettings;
        private readonly IApiClient _apiClient;
        private readonly IMyFollowData _myFollowData;

        /// <summary>
        ///
        /// </summary>
        public StockMonitorService(IMemoryCache memoryCache,
            IWeChatService weChatService,
            IOptions<AppConfig> options,
            IApiClient apiClient,
            IMyFollowData myFollowData,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _weChatService = weChatService;
            _appSettings = options.Value;
            _apiClient = apiClient;
            this._myFollowData = myFollowData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task SendMessageAsync()
        {
            Logger.WriteLog(LogLevel.Info, "开始执行服务", _appSettings);

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
                        var time = _memoryCache.Get<DateTime>(cacheKey);
                        if (time < DateTime.Today)
                        {
                            time = DateTime.Today;
                        }
                        var list = stockData.Where(x => x.TradeTime > time).ToList();
                        if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                var message = $"{item.TradeTime}\n{item.TradeTypeName}：{item.SecuName}({item.StockCode})\n委托价：{item.EntrustPriceStr}元({item.EntrustAmt}股)，撤单{item.CancleAmt}股\n成交价：{item.DealPriceStr}元({item.DealAmountStr}股)\n状态：{item.StatusMsg}\n成交仓位：{item.DealPosition};\n\n";
                                var toUsers = userData.Where(x => x.StockPool == poolName || x.StockCode == item.StockCode).Select(x => x.UserName).Distinct().ToList();
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

                                            await _weChatService.Notice(request);
                                        }
                                    }
                                }
                            }
                        }
                        time = stockData.Max(x => x.TradeTime);
                        _memoryCache.Set(cacheKey, time, TimeSpan.FromDays(1));
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
        /// <returns></returns>
        public async Task SendMessage()
        {
            Logger.WriteLog(LogLevel.Info, "开始执行服务", _appSettings);

            var w = DateTime.Now.DayOfWeek;
            var h = DateTime.Now.Hour;
            if (w == DayOfWeek.Saturday || w == DayOfWeek.Sunday || h < _appSettings.StartHour || h > _appSettings.EndHour)
            {
                return;
            }

            if (_appSettings.StockPoolList == null || _appSettings.StockPoolList.Count <= 0)
            {
                return;
            }

            foreach (var item in _appSettings.StockPoolList)
            {
                var message = await GetStockTradeInfo(item.Name);

                Logger.WriteLog(LogLevel.Info, "读取消息", new { item, message });

                if (!string.IsNullOrEmpty(message))
                {
                    var request = new WechatMessageRequest()
                    {
                        ToTag = item.ToTag,
                        ToUser = item.ToUser,
                        ToParty = item.ToParty,
                        MsgType = NoticeType.text.ToString(),
                        Text = new NoticeText { Content = $"{item.Name}\n{message}" }
                    };

                    try
                    {
                        await _weChatService.Notice(request);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(LogLevel.Error, "发送微信消息通知异常", request, ex);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<string> GetStockTradeInfo(string name)
        {
            var result = "";
            try
            {
                var data = await GetStockTradeListByNameAsync(name);
                if (data != null && data.Count > 0)
                {
                    var cacheKey = $"stocktrade:time:{name.Md5()}";
                    var time = _memoryCache.Get<DateTime>(cacheKey);
                    if (time < DateTime.Today)
                    {
                        time = DateTime.Today;
                    }
                    var list = data.Where(x => x.TradeTime > time).ToList();
                    if (list != null && list.Count > 0)
                    {
                        list.ForEach(item =>
                        {
                            result += $"{item.TradeTime}\n{item.TradeTypeName}：{item.SecuName}({item.StockCode})\n委托价：{item.EntrustPriceStr}元({item.EntrustAmt}股)，撤单{item.CancleAmt}股\n成交价：{item.DealPriceStr}元({item.DealAmountStr}股)\n状态：{item.StatusMsg}\n成交仓位：{item.DealPosition};\n\n";
                        });
                    }
                    time = data.Max(x => x.TradeTime);
                    _memoryCache.Set(cacheKey, time, TimeSpan.FromDays(1));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevel.Error, "读取好股数据异常", name, ex);
            }
            return result;
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
                        o.StockCode == x.StockCode &&
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