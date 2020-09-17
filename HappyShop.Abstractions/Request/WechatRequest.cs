using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class UserReuqest
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户手机号(登录名)
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WechatRequest : UserReuqest
    {
        /// <summary>
        /// 配置账号ID
        /// </summary>
        public int AcountId { get; set; }
        /// <summary>
        /// 加密向量
        /// </summary>
        public string Iv { get; set; }
        /// <summary>
        /// 小程序手机加密数据
        /// </summary>
        public string EncryptedData { get; set; }
        /// <summary>
        /// 微信临时code
        /// </summary>
        public string Code { get; set; }
    }
}
