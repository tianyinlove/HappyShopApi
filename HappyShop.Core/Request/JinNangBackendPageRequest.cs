using HappyShop.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.Model.Page;

namespace HappyShop.Request
{
    /// <summary>
    /// 排序规则
    /// </summary>
    public enum JinNangPageSortKind
    {
        /// <summary>
        /// 按默认排序，推荐锦囊按权重排序，传Default
        /// </summary>
        Default = -1,

        /// <summary>
        /// 总收益排序
        /// </summary>
        TotalYeild = 0,

        /// <summary>
        /// 日期收益
        /// </summary>
        DayYeild = 1,

        /// <summary>
        /// 按周收益排序
        /// </summary>
        WeekYeild = 2,

        /// <summary>
        /// 月收益
        /// </summary>
        MonthYeild = 3,

        /// <summary>
        /// 季收益
        /// </summary>
        SeasonYeild = 4,

        /// <summary>
        /// 年收益
        /// </summary>
        YearYeild = 5,

        /// <summary>
        /// 按创建日期排序
        /// </summary>
        CreateTime = 11,

        /// <summary>
        /// 按打理日期排序
        /// </summary>
        LastOperateTime = 10,

        /// <summary>
        /// 推荐列表特殊处理,同 Default
        /// </summary>
        Recommend = 99
    }

    /// <summary>
    /// 查询类型
    /// </summary>
    public enum JinNangQueryType
    {
        /// <summary>
        /// 我的锦囊
        /// </summary>
        Mine = 0,

        /// <summary>
        /// 所有锦囊
        /// </summary>
        All = 1,

        /// <summary>
        /// 精选锦囊
        /// </summary>
        Recommend = 2,

        /// <summary>
        /// 热门锦囊
        /// </summary>
        Hot = 3,

        /// <summary>
        /// 走心推荐
        /// </summary>
        HeartFeletRecommend = 4,

        /// <summary>
        /// 管理后台查询所有锦囊(字段全)
        /// </summary>
        QueryFromBackEnd = 9,

        /// <summary>
        /// 组合锦囊
        /// </summary>
        JinNangPackage = 10
    }

    /// <summary>
    ///
    /// </summary>
    public class JinNangBackendPageRequest : IndexPageRequest
    {
        /// <summary>
        /// 风险等级
        /// </summary>
        public List<RiskLevel> Levels { get; set; }

        /// <summary>
        /// 风格
        /// </summary>
        public List<JinNangStyle> Styles { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public JinNangPageSortKind SortKind { get; set; }

        /// <summary>
        /// 查询推荐锦囊
        /// </summary>
        public JinNangQueryType QueryType { get; set; }

        /// <summary>
        /// 锦囊所属大咖ID
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// 锦囊大咖Id列表
        /// </summary>
        public List<int> AuthorIds { get; set; }

        /// <summary>
        /// 锦囊Id列表
        /// </summary>
        public List<int> JinNangIds { get; set; }

        /// <summary>
        /// 锦囊名称
        /// </summary>
        public string JinNangName { get; set; }

        /// <summary>
        /// 大咖名称
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        ///  -1全部/10未生效/0待上架/1销售中/3已下架/11已到期（只有prodtype=9适用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 大咖的运营分组
        /// 对应老师的运营分组: -1全部；1今日股市/2谈股论金/3公司与行业/4市场零距离/0其他 （只有prodtype=9适用）
        /// </summary>
        public int Opergroup { get; set; }
    }
}