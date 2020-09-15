using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Documents
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDocument
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
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
        /// 用户邮箱(登录名) 用户唯一标识
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdateTime { get; set; }
    }
}
