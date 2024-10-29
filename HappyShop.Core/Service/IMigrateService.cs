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
    public interface IMigrateService
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task InitDataAsync();
    }
}