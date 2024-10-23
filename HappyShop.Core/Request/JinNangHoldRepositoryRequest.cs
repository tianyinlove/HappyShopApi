using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Model.Page;

namespace HappyShop.Request
{
    /// <summary>
    /// 持仓查询请求
    /// </summary>
    public class JinNangHoldRepositoryRequest : SlidePageRequest
    {
        /// <summary>
        /// 开始版本号
        /// </summary>
        public long FromVersion { get; set; }

        /// <summary>
        /// 开始索引
        /// </summary>
        public long FromIndex { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType OrderBy { get; set; }

        /// <summary>
        /// 锦囊Id
        /// </summary>
        public int JinNangId { get; set; }

        /// <summary>
        /// 市场代码
        /// </summary>
        public Market Market { get; set; }

        /// <summary>
        /// 证券代码
        /// </summary>
        public string SecuritiesCode { get; set; }

        /// <summary>
        /// 带市场股票代码（和market、secucode 二选一）
        /// </summary>
        public int StockCode { get; set; }
    }
}