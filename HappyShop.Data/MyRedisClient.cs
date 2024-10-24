using HappyShop.Comm;
using HappyShop.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions.Redis;

namespace HappyShop.Data
{
    /// <summary>
    ///
    /// </summary>
    internal class MyRedisClient : RedisClientBase, IMyRedisClient
    {
        private readonly ILogger<MyRedisClient> _logger;
        private readonly IOptionsMonitor<AppConfig> _optionsMonitor;

        /// <summary>
        ///
        /// </summary>
        public MyRedisClient(ILogger<MyRedisClient> logger, IOptionsMonitor<AppConfig> optionsMonitor)
        {
            _logger = logger;
            _optionsMonitor = optionsMonitor;
        }

        protected override string ClientName => "happyshopapi";

        public override string ConnectionString => _optionsMonitor.CurrentValue.RedisConnectionString;

        protected override ILogger Logger => _logger;

        public override string KeyPrefix => "happyshop:api";
    }
}