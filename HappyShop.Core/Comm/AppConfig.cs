using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Comm
{
    /// <summary>
    ///
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        ///
        /// </summary>
        public int StartHour { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int EndHour { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string TradeUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// mongodb配置
        /// </summary>
        public MongodbConfig MongodbConfig { get; set; }

        /// <summary>
        /// 微信配置
        /// </summary>
        public WechatConfig WechatConfig { get; set; }

        /// <summary>
        /// 企业微信配置
        /// </summary>
        public QYWechatConfig QYWechatConfig { get; set; }

        /// <summary>
        /// 微信账号配置
        /// </summary>
        public List<WechatAccount> WechatAccount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<StockPoolConfig> StockPoolList { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class StockPoolConfig
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接收标签ID，多个用|分隔，最多支持1000个
        /// </summary>
        public string ToTag { get; set; }

        /// <summary>
        /// 接收成员ID，多个用|分隔，最多支持1000个
        /// </summary>
        public string ToUser { get; set; }

        /// <summary>
        /// 接收部门ID，多个用|分隔，最多支持1000个
        /// </summary>
        public string ToParty { get; set; }
    }

    /// <summary>
    /// mongodb配置
    /// </summary>
    public class MongodbConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
    }

    /// <summary>
    /// 微信配置
    /// </summary>
    public class WechatConfig
    {
        /// <summary>
        /// 微信App端连接获取code的Url
        /// </summary>
        public string WechatAppConnect { get; set; }

        /// <summary>
        /// 微信PC端连接获取code的Url
        /// </summary>
        public string WechatPCConnect { get; set; }

        /// <summary>
        /// 微信获取临时token用户Api地址
        /// </summary>
        public string WechatTokenUrl { get; set; }

        /// <summary>
        /// 微信获取用户信息Api地址
        /// </summary>
        public string WechatUserUrl { get; set; }

        /// <summary>
        /// 小程序获取临时tokenApi地址
        /// </summary>
        public string MiniTokenUrl { get; set; }

        /// <summary>
        /// 小程序获取登录状态Api地址
        /// </summary>
        public string MiniSessionUrl { get; set; }

        /// <summary>
        /// 小程序获取用户信息Api地址
        /// </summary>
        public string MiniUserUrl { get; set; }

        /// <summary>
        /// 微信获取jsapi_ticket
        /// </summary>
        public string WechatTicketUrl { get; set; }
    }

    /// <summary>
    /// 企业微信配置
    /// </summary>
    public class QYWechatConfig
    {
        /// <summary>
        /// 微信App端连接获取code的Url
        /// </summary>
        public string WechatAppConnect { get; set; }

        /// <summary>
        /// 微信PC端连接获取code的Url
        /// </summary>
        public string WechatPCConnect { get; set; }

        /// <summary>
        /// 微信获取临时token用户Api地址
        /// </summary>
        public string WechatTokenUrl { get; set; }

        /// <summary>
        /// 微信获取用户信息Api地址
        /// </summary>
        public string WechatUserUrl { get; set; }

        /// <summary>
        /// code换取用户信息Api地址
        /// </summary>
        public string WechatTicketUrl { get; set; }

        /// <summary>
        /// 微信发送消息Api地址
        /// </summary>
        public string WechatSendUrl { get; set; }
    }

    /// <summary>
    /// 微信账号配置
    /// </summary>
    public class WechatAccount
    {
        /// <summary>
        /// 账号配置ID
        /// </summary>
        public int AcountId { get; set; }

        /// <summary>
        /// 微信账号类型(1:小程序;2:公众号;)
        /// </summary>
        public int AcountType { get; set; }

        /// <summary>
        /// 微信appid
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 用户授权回调
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 用户微信授权成功跳转链接
        /// </summary>
        public string SuccessUrl { get; set; }
    }
}