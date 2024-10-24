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
    public class QYWechatResponse
    {
        /// <summary>
        /// 0表示成功，非0表示失败
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ErrMsg { get; set; }
    }
}