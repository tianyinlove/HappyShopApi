using HappyShop.Domian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Response
{
    /// <summary>
    /// 持仓
    /// </summary>
    public class JinNangHoldRepositoryWrapper
    {
        /// <summary>
        /// 列表项
        /// </summary>
        public List<HoldRepositoryItem> Items { get; set; }
    }
}