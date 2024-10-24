using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Domian
{
    /// <summary>
    ///
    /// </summary>
    public class QYWechatLoginUser : QYWechatResponse
    {
        /// <summary>
        /// 用户所属企业的corpid
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// 用户在企业内的UserID，对应管理端的账号，企业内唯一。注意：第三方小程序此处返回加密的userid
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 会话密钥
        /// </summary>
        public string Session_Key { get; set; }
    }
}