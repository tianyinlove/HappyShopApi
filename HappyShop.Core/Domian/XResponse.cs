﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Domian
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [ProtoContract()]
    public class XResponse
    {
        /// <summary>
        /// 状态
        /// </summary>
        [ProtoMember(1)]
        public XResult Result { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [ProtoMember(2, Name = "detail")]
        public ProtoAny Detail { get; set; }
    }

    /// <summary>
    /// 状态定义
    /// </summary>
    [ProtoContract()]
    public class XResult
    {
        /// <summary>
        ///
        /// </summary>
        [ProtoMember(1)]
        public int Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        [ProtoMember(2)]
        public string Msg { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [ProtoContract]
    public sealed class ProtoAny
    {
        /// <summary>
        ///
        /// </summary>
        [ProtoMember(1, Name = "type_url")]
        public string TypeUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        [ProtoMember(2, Name = "value")]
        public byte[] Value { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class StockTradeInfo
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 证券名称
        /// </summary>
        public string Secuname { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 建仓,加仓,减仓,平仓,撤单,送红股,派息,配股
        /// </summary>

        public string Busimsg { get; set; }

        /// <summary>
        /// 成交数量 (股)
        /// </summary>
        public long DealAmount { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public decimal DealPrice { get; set; }

        /// <summary>
        /// 委托价格  	*10000
        /// </summary>
        public decimal EntrustPrice { get; set; }

        /// <summary>
        /// 成交占比
        /// </summary>
        public string DealPosition { get; set; }

        /// <summary>
        /// 委托数量 (股)
        /// </summary>
        public long Entrustamt { get; set; }

        /// <summary>
        /// 撤单数量 (股)
        /// </summary>
        public long Cancelamt { get; set; }

        /// <summary>
        /// 原来仓位
        /// </summary>
        public string Stkpospre { get; set; }

        /// <summary>
        /// 目标仓位
        /// </summary>
        public string Stkposdst { get; set; }
    }

    /// <summary>
    /// 持仓
    /// </summary>
    public class HoldRepositoryItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// 最后一次操作：建仓、加仓、减仓、平仓
        /// </summary>
        public string BusMsg { get; set; }

        /// <summary>
        /// 证券代码：
        /// </summary>
        public string SecuritiesCode { get; set; }

        /// <summary>
        /// 证券名称
        /// </summary>
        public string SecuritiesName { get; set; }

        /// <summary>
        /// 买入成本 *10000
        /// </summary>
        public int SecuCost { get; set; }

        /// <summary>
        /// 最新价 *10000
        /// </summary>
        public int NewPrice { get; set; }

        /// <summary>
        /// 最新价
        /// </summary>
        public string NewPriceStr { get; set; }

        /// <summary>
        /// 摊薄成本价 	*10000
        /// </summary>
        public int DilucostPrice { get; set; }

        /// <summary>
        /// 摊薄成本价
        /// </summary>
        public string DilucostPriceStr { get; set; }

        /// <summary>
        /// 持仓比例，仓位 *10000
        /// </summary>
        public int SecuScale { get; set; }

        /// <summary>
        /// 浮动盈亏比例 *10000
        /// </summary>
        public string GainLossScale { get; set; }

        /// <summary>
        /// 总盈亏*10000
        /// </summary>
        public long TotalGailLoss { get; set; }

        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 行业ID
        /// </summary>
        public int IndustryId { get; set; }

        /// <summary>
        /// 市值 * 10000
        /// </summary>
        public long MarketValue { get; set; }

        /// <summary>
        /// 当前持仓数量
        /// </summary>
        public int SecuAmount { get; set; }

        /// <summary>
        /// 当前持仓数量
        /// </summary>
        public string SecuAmuntStr { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public int UsableAmount { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public string UsableAmountStr { get; set; }

        /// <summary>
        /// 持仓比例
        /// </summary>
        public string SecuScaleStr { get; set; }
    }
}