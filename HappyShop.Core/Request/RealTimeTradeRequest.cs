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
    /// 买卖过滤
    /// </summary>
    public enum BsFilterType
    {
        /// <summary>
        /// 全部
        /// </summary>
        None = 0,

        /// <summary>
        /// 买入
        /// </summary>
        Buy = 1,

        /// <summary>
        /// 卖出
        /// </summary>
        Sell = 2
    }

    /// <summary>
    ///
    /// </summary>
    public enum TradeFilterType
    {
        /// <summary>
        /// 全部
        /// </summary>
        None = 0,

        /// <summary>
        /// 成交
        /// </summary>
        Completed = 1,

        /// <summary>
        /// 未成交(包括撤单)
        /// </summary>
        NotCompleted = 2
    }

    /// <summary>
    ///
    /// </summary>
    public enum Market
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = 0,

        /// <summary>
        /// 深圳
        /// </summary>
        ShenZhen = 1,

        /// <summary>
        /// 上海
        /// </summary>
        ShangHai = 2,
    }

    /// <summary>
    /// 实时调仓记录请求
    /// </summary>
    public class RealTimeTradeRequest : SlidePageRequest
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
        /// 锦囊ID
        /// </summary>
        public int JinNangId { get; set; }

        /// <summary>
        /// 锦囊组合Id
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// //市场代码：0:全部；1:深圳；2:上海
        /// </summary>
        public Market Market { get; set; }

        /// <summary>
        /// //证券代码：空则查全部
        /// </summary>
        public string SecuritiesCode { get; set; }

        /// <summary>
        /// 开始日期,格式20170524， 0表示当日
        /// </summary>
        public long BeginTme { get; set; } = Convert.ToInt64(DateTime.Today.ToString("yyyyMMdd"));

        /// <summary>
        /// 结束日期,格式20170524,  0表示当日
        /// </summary>
        public long EndTime { get; set; } = Convert.ToInt64(DateTime.Today.ToString("yyyyMMdd"));

        /// <summary>
        /// 是否成交过滤条件,(在@p_status=1有效)， 0全部，1成交，2未成交(包括撤单)
        /// </summary>
        public TradeFilterType Filter { get; set; } = TradeFilterType.None;

        /// <summary>
        /// 交易类型
        /// </summary>
        public BsFilterType TradeType { get; set; } = BsFilterType.None;

        /// <summary>
        /// 委托单号 填0表示查询所有的，>0表示指定的委托号
        /// </summary>
        public long EntrustNo { get; set; }

        /// <summary>
        /// 带市场股票代码（和market、secucode 二选一）
        /// </summary>
        public int StockCode { get; set; }
    }
}