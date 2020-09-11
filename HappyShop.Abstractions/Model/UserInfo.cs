using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Unionid
        /// </summary>
        public string Unionid { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 用户手机号(登录名)
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户邮箱(登录名)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
