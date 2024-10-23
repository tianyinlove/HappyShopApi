using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HappyShop.Model
{
    /// <summary>
    /// 锦囊列表项
    /// </summary>
    public class JinNangListItem
    {
        /// <summary>
        /// 锦囊名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创建日期时间戳
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 打理日期
        /// </summary>
        public long LastOperateTime { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// 锦囊所属老师名称
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 老师头衔
        /// </summary>
        public string AuthorTitle { get; set; }

        /// <summary>
        /// 老师公司
        /// </summary>
        public string AuthorCompany { get; set; }

        /// <summary>
        /// 大咖头像地址
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 大咖Id
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// 锦囊ID
        /// </summary>
        public int JinNangId { get; set; }

        /// <summary>
        /// 总收益
        /// </summary>
        public string GlYield { get; set; }

        /// <summary>
        /// 近一月收益
        /// </summary>
        public string MonthYield { get; set; }

        /// <summary>
        /// 三月收益率（季收益率）
        /// </summary>
        public string QrtYield { get; set; }

        /// <summary>
        /// 年收益率 		*10000
        /// </summary>
        public string YearYield { get; set; }

        /// <summary>
        /// 日收益
        /// </summary>
        public string DayYield { get; set; }

        /// <summary>
        /// 周收益
        /// </summary>
        public string WeekYield { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        public RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// 锦囊风格
        /// </summary>
        public JinNangStyle Style { get; set; }

        /// <summary>
        /// 锦囊风格名称
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 回撤率
        /// </summary>
        public string Maxdrawidx { get; set; }

        /// <summary>
        /// 胜率
        /// </summary>
        public string Selwinidx { get; set; }

        /// <summary>
        /// 锦囊介绍
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 大咖运营分组
        /// </summary>
        public int OperGroup { get; set; }

        /// <summary>
        /// 大咖运营分组名称
        /// </summary>
        public string OperGroupName { get; set; }

        /// <summary>
        /// 运营状态字符串表示
        /// </summary>
        public string StatusStr { get; set; }

        /// <summary>
        /// 开通时间
        /// </summary>
        public string ValidBeginTime { get; set; }

        /// <summary>
        /// 锦囊上架时间
        /// </summary>
        public string PublishTime { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 推荐权重
        /// </summary>
        public int RecoWeight { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public int? ProductId { get; set; }

        ///锦囊属性，按位存储。
        ///0x01：1热门；0非热门；
        ///0x02：1精选；0非精选；
        ///0x04表示是否上架：1上架，0下架；
        ///0x08表示是否售罄：1已售罄，0未售罄。
        public JinNangProperty Property { get; set; }

        /// <summary>
        /// 是否精选
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.Property.HasFlag(JinNangProperty.Select);
            }
        }

        /// <summary>
        /// 持仓总仓位
        /// </summary>
        public string MktLevel { get; set; }
    }

    /// <summary>
    /// 锦囊属性
    /// </summary>
    [Flags]
    public enum JinNangProperty
    {
        /// <summary>
        /// 热门
        /// </summary>
        Hot = 1,

        /// <summary>
        /// 精选
        /// </summary>
        Select = 2,

        /// <summary>
        /// 上架
        /// </summary>
        OnShelf = 4,

        /// <summary>
        /// 售罄
        /// </summary>
        Saleout = 8
    }

    /// <summary>
    /// 锦囊风格 0其它/1波段操作/2中短线/3中长线/4基本面/5趋势交易/6量化投资/7价值成长/8热点题材
    /// </summary>
    public enum JinNangStyle
    {
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        None = 0,

        /// <summary>
        /// 波段操作
        /// </summary>
        [Description("波段操作")]
        BandOperation = 1,

        /// <summary>
        /// 中短线
        /// </summary>
        [Description("中短线")]
        MediumShortLine = 2,

        /// <summary>
        /// 中长线
        /// </summary>
        [Description("中长线")]
        MediumLongLine = 3,

        /// <summary>
        /// 基本面
        /// </summary>
        [Description("基本面")]
        Fundamentals = 4,

        /// <summary>
        /// 趋势交易
        /// </summary>
        [Description("趋势交易")]
        TrendTrading = 5,

        /// <summary>
        /// 量化投资
        /// </summary>
        [Description("量化投资")]
        QuantInvestment = 6,

        /// <summary>
        /// 价值成长
        /// </summary>
        [Description("价值成长")]
        ValueGrowth = 7,

        /// <summary>
        /// 热点题材
        /// </summary>
        [Description("热点题材")]
        HotTopics = 8
    }

    /// <summary>
    /// 风险等级
    /// </summary>
    public enum RiskLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 稳健
        /// </summary>
        [Description("稳健型")]
        Steady = 1,

        /// <summary>
        /// 积极型
        /// </summary>
        [Description("积极型")]
        Positive = 2,

        /// <summary>
        /// 激进
        /// </summary>
        [Description("激进型")]
        Radical = 3
    }

    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 样式
        /// </summary>

        public TagStyle Style { get; set; }
    }

    public enum TagStyle
    {
        /// <summary>
        /// 白色背景蓝色边框蓝色字体
        /// </summary>
        WhiteBackgroundColorBlue = 1,
    }
}