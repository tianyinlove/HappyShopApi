using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Model;
using Utility.NetLog;

namespace NoticeWorkerService
{
    /// <summary>
    ///
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        ///
        /// </summary>
        public string TradeUrl { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsMonitor<AppConfig> _options;

        /// <summary>
        ///
        /// </summary>
        public Worker(IHttpClientFactory httpClientFactory,
            IOptionsMonitor<AppConfig> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, _options.CurrentValue.TradeUrl))
                    {
                        using (var response = await _httpClientFactory.CreateClient().SendAsync(request))
                        {
                            await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevel.Error, "服务异常", ex);
                }
                await Task.Delay(5 * 1000, stoppingToken);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.WriteLog(LogLevel.Info, "启动服务");
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.WriteLog(LogLevel.Info, "关闭服务");
            return base.StopAsync(cancellationToken);
        }
    }
}