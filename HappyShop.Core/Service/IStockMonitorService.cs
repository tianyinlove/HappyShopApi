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
    public interface IStockMonitorService
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task SendMessageAsync(int accountId);
    }
}