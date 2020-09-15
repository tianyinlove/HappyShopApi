using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Domian
{
    /// <summary>
    /// 公众号授权Token
    /// </summary>
    public class WeChatAccessTokenInfo
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int Expires_In { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string Refresh_Token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// 公众号用户信息
    /// </summary>
    public class WeChatUserInfo
    {
        /// <summary>
        /// 统一平台下用户唯一标识
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string[] Privilege { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// JSAPI授权Ticket
    /// </summary>
    public class WechatTicketInfo
    {
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int Expires_In { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ticket { get; set; }
    }
}
