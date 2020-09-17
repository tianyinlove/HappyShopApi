using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HappyShop.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Unionid 用户唯一标识
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// OpenId
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
        /// 用户手机号(登录名) 用户唯一标识
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string PassWord { get; set; }
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
