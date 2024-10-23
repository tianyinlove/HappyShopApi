using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Model
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradeType
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
        Sell = 2,

        /// <summary>
        /// 其它
        /// </summary>
        Other = 3,

        /// <summary>
        /// 撤单
        /// </summary>
        CheDan = 4,

        /// <summary>
        /// 送红股
        /// </summary>
        SongHongGu = 5,

        /// <summary>
        /// 派息
        /// </summary>
        PaiXi = 6,

        /// <summary>
        /// 配股上市
        /// </summary>
        PeiGuShangShi = 7,

        /// <summary>
        /// 配股缴款
        /// </summary>
        PeiGuJiaoKuan = 8,

        /// <summary>
        /// 配股除权
        /// </summary>
        PeiGuChuQuan = 9,

        /// <summary>
        /// 加入锦囊组合
        /// </summary>
        JoinPackge = 10,

        /// <summary>
        /// 调出锦囊组合
        /// </summary>
        LeavePackage = 11
    }
}