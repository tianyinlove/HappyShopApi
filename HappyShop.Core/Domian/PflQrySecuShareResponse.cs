using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HappyShop.Domian
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoBuf.ProtoContract(Name = @"PflQrySecuShare_Response")]
    public class PflQrySecuShareResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoBuf.ProtoMember(1, Name = @"request_params")]
        [JsonProperty("requestParams")]
        public PflQrySecuShareRequest RequestParams { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        [ProtoBuf.ProtoMember(2, Name = @"secushare")]
        [JsonProperty("secushare")]
        public System.Collections.Generic.List<SecuShare> Secushare { get; set; } = new System.Collections.Generic.List<SecuShare>();

        /// <summary>
        /// 总记录数：
        /// </summary>
        [ProtoBuf.ProtoMember(3, Name = @"totalnum")]
        [JsonProperty("totalnum")]
        public uint Totalnum { get; set; }

        /// <summary>
        /// 总仓位 *10000
        /// </summary>
        [ProtoBuf.ProtoMember(4, Name = @"stkscale")]
        [JsonProperty("stkscale")]
        public uint Stkscale { get; set; }

        [ProtoBuf.ProtoContract()]
        public class SecuShare
        {
            /// <summary>
            /// 序号
            /// </summary>
            [ProtoBuf.ProtoMember(1, Name = @"idx")]
            [JsonProperty("idx")]
            public uint Idx { get; set; }

            /// <summary>
            /// 最后一次操作：G建仓、A加仓、D减仓、E平仓
            /// </summary>
            [ProtoBuf.ProtoMember(2, Name = @"busiflag")]
            [JsonProperty("busiflag")]
            [System.ComponentModel.DefaultValue("")]
            public string Busiflag { get; set; } = "";

            /// <summary>
            /// 最后一次操作：建仓、加仓、减仓、平仓
            /// </summary>
            [ProtoBuf.ProtoMember(3, Name = @"busimsg")]
            [JsonProperty("busimsg")]
            [System.ComponentModel.DefaultValue("")]
            public string Busimsg { get; set; } = "";

            /// <summary>
            /// 市场代码：1:深圳；2:上海
            /// </summary>
            [ProtoBuf.ProtoMember(4, Name = @"market")]
            [JsonProperty("market")]
            public int Market { get; set; }

            /// <summary>
            /// 证券代码：
            /// </summary>
            [ProtoBuf.ProtoMember(5, Name = @"secucode")]
            [JsonProperty("secucode")]
            [System.ComponentModel.DefaultValue("")]
            public string Secucode { get; set; } = "";

            /// <summary>
            /// 证券名称
            /// </summary>
            [ProtoBuf.ProtoMember(6, Name = @"secuname")]
            [JsonProperty("secuname")]
            [System.ComponentModel.DefaultValue("")]
            public string Secuname { get; set; } = "";

            /// <summary>
            /// 最新价 			*10000
            /// </summary>
            [ProtoBuf.ProtoMember(7, Name = @"newprice")]
            [JsonProperty("newprice")]
            public uint Newprice { get; set; }

            /// <summary>
            /// 买入成本(无效)		*10000
            /// </summary>
            [ProtoBuf.ProtoMember(8, Name = @"secucost")]
            [JsonProperty("secucost")]
            public uint Secucost { get; set; }

            /// <summary>
            /// 摊薄成本价 	*10000
            /// </summary>
            [ProtoBuf.ProtoMember(9, Name = @"dilucostprice", DataFormat = ProtoBuf.DataFormat.ZigZag)]
            [JsonProperty("dilucostprice")]
            public int Dilucostprice { get; set; }

            /// <summary>
            /// 保本价 		*10000
            /// </summary>
            [ProtoBuf.ProtoMember(10, Name = @"breakevenprice")]
            [JsonProperty("breakevenprice")]
            public uint Breakevenprice { get; set; }

            /// <summary>
            /// 盈亏比例  		*10000
            /// </summary>
            [ProtoBuf.ProtoMember(11, Name = @"plscale", DataFormat = ProtoBuf.DataFormat.ZigZag)]
            [JsonProperty("plscale")]
            public int Plscale { get; set; }

            /// <summary>
            /// 浮动盈亏比例  	*10000
            /// </summary>
            [ProtoBuf.ProtoMember(12, Name = @"unplscale", DataFormat = ProtoBuf.DataFormat.ZigZag)]
            [JsonProperty("unplscale")]
            public int Unplscale { get; set; }

            /// <summary>
            /// 持仓比例  		*10000
            /// </summary>
            [ProtoBuf.ProtoMember(13, Name = @"secuscale")]
            [JsonProperty("secuscale")]
            public uint Secuscale { get; set; }

            /// <summary>
            /// 建仓日期 20170508
            /// </summary>
            [ProtoBuf.ProtoMember(14, Name = @"opendate")]
            [JsonProperty("opendate")]
            public uint Opendate { get; set; }

            /// <summary>
            /// 更新时间(yyyy-mm-dd HH:mm:ss)
            /// </summary>
            [ProtoBuf.ProtoMember(15, Name = @"uptime")]
            [JsonProperty("uptime")]
            [System.ComponentModel.DefaultValue("")]
            public string Uptime { get; set; } = "";

            /// <summary>
            /// 行业id
            /// </summary>
            [ProtoBuf.ProtoMember(16, Name = @"indusid")]
            [JsonProperty("indusid")]
            public uint Indusid { get; set; }

            /// <summary>
            /// 行业名称
            /// </summary>
            [ProtoBuf.ProtoMember(17, Name = @"indusname")]
            [JsonProperty("indusname")]
            [System.ComponentModel.DefaultValue("")]
            public string Indusname { get; set; } = "";

            /// <summary>
            /// 当前股票涨跌幅  		*10000
            /// </summary>
            [ProtoBuf.ProtoMember(18, Name = @"secupl")]
            [JsonProperty("secupl")]
            public int Secupl { get; set; }

            /// <summary>
            /// 带市场股票代码 （和market、secucode 二选一）
            /// </summary>
            [ProtoBuf.ProtoMember(19, Name = @"stockcode")]
            [JsonProperty("stockcode")]
            public uint Stockcode { get; set; }

            /// <summary>
            /// 市值 *1000 一千		[rev: 资金借出时是付出的资金(不包含手续费);资金回款时是包含收益的总金额 *1000]
            /// </summary>
            [ProtoBuf.ProtoMember(20, Name = @"marketvalue")]
            [JsonProperty("marketvalue")]
            public ulong Marketvalue { get; set; }

            /// <summary>
            /// 当前持仓数量 股		[rev: 卖出的数量，手、张]
            /// </summary>
            [ProtoBuf.ProtoMember(21, Name = @"secuamount")]
            [JsonProperty("secuamount")]
            public uint Secuamount { get; set; }

            /// <summary>
            /// 可用数量 股		[rev: 0]
            /// </summary>
            [ProtoBuf.ProtoMember(22, Name = @"usableamount")]
            [JsonProperty("usableamount")]
            public uint Usableamount { get; set; }

            /// <summary>
            /// 冻结数量 股		[rev: 卖出的数量，手、张]
            /// </summary>
            [ProtoBuf.ProtoMember(23, Name = @"frozenamount")]
            [JsonProperty("frozenamount")]
            public uint Frozenamount { get; set; }

            /// <summary>
            /// 累计买入金额 *1000 一千		[rev: 0]
            /// </summary>
            [ProtoBuf.ProtoMember(24, Name = @"totalbuymoney")]
            [JsonProperty("totalbuymoney")]
            public ulong Totalbuymoney { get; set; }

            /// <summary>
            /// 累计买入数量 股					[rev: 0]
            /// </summary>
            [ProtoBuf.ProtoMember(25, Name = @"totalbuyamt")]
            [JsonProperty("totalbuyamt")]
            public uint Totalbuyamt { get; set; }

            /// <summary>
            /// 累计卖出金额 *1000 一千		[rev: 付出的金额，包括手续费]
            /// </summary>
            [ProtoBuf.ProtoMember(26, Name = @"totalsalemoney")]
            [JsonProperty("totalsalemoney")]
            public ulong Totalsalemoney { get; set; }

            /// <summary>
            /// 累计卖出数量 股				[rev: 卖出的数量，手、张]
            /// </summary>
            [ProtoBuf.ProtoMember(27, Name = @"totalsaleamt")]
            [JsonProperty("totalsaleamt")]
            public uint Totalsaleamt { get; set; }

            /// <summary>
            /// 总盈亏 *1000 一千		[rev: 收益、利息]
            /// </summary>
            [ProtoBuf.ProtoMember(28, Name = @"totalpl", DataFormat = ProtoBuf.DataFormat.ZigZag)]
            [JsonProperty("totalpl")]
            public long Totalpl { get; set; }

            /// <summary>
            /// 可撤单数量 （请求填market,secucode才返回有效值） [rev: yyyymmdd回款日期]
            /// </summary>
            [ProtoBuf.ProtoMember(29, Name = @"cancelableamt")]
            [JsonProperty("cancelableamt")]
            public uint Cancelableamt { get; set; }

            /// <summary>
            /// 昨收价 *10000			[rev: 0]
            /// </summary>
            [ProtoBuf.ProtoMember(30, Name = @"preclose")]
            [JsonProperty("preclose")]
            public uint Preclose { get; set; }

            /// <summary>
            /// 附加提示信息（目前有配股的，在期间进行提示  2018.03.02）
            /// </summary>
            [ProtoBuf.ProtoMember(31, Name = @"exmsg")]
            [JsonProperty("exmsg")]
            [System.ComponentModel.DefaultValue("")]
            public string Exmsg { get; set; } = "";

        }
    }
}
