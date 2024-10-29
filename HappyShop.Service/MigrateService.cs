using HappyShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    internal class MigrateService : IMigrateService
    {
        private readonly IHappyShopMongoContext _mongodb;
        private readonly IMyRedisClient _redis;

        /// <summary>
        ///
        /// </summary>
        public MigrateService(IHappyShopMongoContext mongodb,
            IMyRedisClient redis)
        {
            _mongodb = mongodb;
            _redis = redis;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task InitDataAsync()
        {
            // 初始化mongodb索引
            await _mongodb.InitQYUserInfoIndexs();
            await _mongodb.InitUserInfoIndexs();
        }
    }
}