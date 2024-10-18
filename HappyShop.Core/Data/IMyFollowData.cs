using HappyShop.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Data
{
    /// <summary>
    ///
    /// </summary>
    public interface IMyFollowData
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MyFollowInfoDocument>> GetMyFollows(string userId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="stockPool">股票池名</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="isFollow">是否关注</param>
        /// <returns></returns>
        Task<bool> SaveUpdate(string userName, string stockPool, string stockCode, bool isFollow);
    }
}