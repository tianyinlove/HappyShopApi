using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Model
{
    /// <summary>
    /// item 项
    /// </summary>
    public class RealTimeTradeItem
    {
        /// <summary>
        /// 交易时间,若为成交 则为委托时间
        /// </summary>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 买入，卖出,撤单,送红股,派息,配股,其它，调入，调出
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// 建仓,加仓,减仓,平仓,撤单,送红股,派息,配股
        /// </summary>
        public string TradeTypeName { get; set; }

        /// <summary>
        /// 成交价 * 10000
        /// </summary>
        public int DealPrice { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public string DealPriceStr { get; set; }

        /// <summary>
        /// 成交股数
        /// </summary>
        public int DealNumber { get; set; }

        /// <summary>
        /// 成交额*10000
        /// </summary>
        public long DealAmount { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public string DealAmountStr { get; set; }

        /// <summary>
        /// 成交占比
        /// </summary>
        public string DealPosition { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperateRemark { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// 证券代码
        /// </summary>
        public string SecuCode { get; set; }

        /// <summary>
        /// 证券名称
        /// </summary>
        public string SecuName { get; set; }

        /// <summary>
        /// 带市场股票代码（）
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 委托合同号
        /// </summary>
        public long EntrustNo { get; set; }

        /// <summary>
        /// 股票ID
        /// </summary>
        public int StockId { get; set; }

        /// <summary>
        /// 状态码 0全部成交(成交价>0)，1全部撤单，2已报[未成交](可撤)，3部分成交，
        /// 4部成部撤，10未报(废弃，现在2表示可撤单)，11废单，12已报[未成交](不可撤)，99未知
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 状态码信息中文字
        /// </summary>
        public string StatusMsg { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        public DateTime DealTime { get; set; }

        /// <summary>
        /// 盈亏 *10000
        /// </summary>
        public long TotalPl { get; set; }

        /// <summary>
        /// 盈亏(0.00)
        /// </summary>
        public string TotalPlStr { get; set; }

        /// <summary>
        /// 盈亏比例
        /// </summary>
        public string PlScale { get; set; }

        /// <summary>
        /// 资金发生数（包含税费）*10000
        /// </summary>
        public long TradeMoney { get; set; }

        /// <summary>
        /// 委托价 * 10000
        /// </summary>
        public long EntrustPrice { get; set; }

        /// <summary>
        /// 委托价(0.00)
        /// </summary>
        public string EntrustPriceStr { get; set; }

        /// <summary>
        /// 委托数量(股)
        /// </summary>
        public int EntrustAmt { get; set; }

        /// <summary>
        /// 撤单数量（股）
        /// </summary>
        public int CancleAmt { get; set; }

        /// <summary>
        /// 委托时间
        /// </summary>
        public DateTime EntrustTime { get; set; }

        /// <summary>
        /// 当前锦囊Id
        /// </summary>
        public int JinNangId { get; set; }

        /// <summary>
        /// 锦囊名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 大咖名称
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 大咖Id
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
    }
}