using HappyShop.Domian;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.Model;

namespace HappyShop.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<HoldRepositoryItem>> GetStockListByIdAsync(int prodid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<StockTradeInfo>> GetStockTradeListByNameAsync(string name);
    }
}
