using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class WechatJSTicket
    {
        /// <summary>
        /// 
        /// </summary>
        public string NonceStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Signature { get; set; }
    }
}
