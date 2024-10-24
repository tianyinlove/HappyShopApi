using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Extensions.Redis;

namespace HappyShop.Repositories
{
    /// <summary>
    /// Redis客户端接口
    /// </summary>
    public interface IMyRedisClient : IRedisClientBase
    {
    }
}