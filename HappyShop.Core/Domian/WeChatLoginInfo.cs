using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Domian
{
    /// <summary>
    /// 小程序登录返回信息
    /// </summary>
    public class WeChatLoginInfo
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符
        /// </summary>
        public string unionid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MiniUserPhone
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 区域手机号
        /// </summary>
        public string PurePhoneNumber { get; set; }
        /// <summary>
        /// 区码
        /// </summary>
        public string CountryCode { get; set; }
    }
}
