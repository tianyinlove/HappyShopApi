using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HappyShop.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultResponse<T> : ResultResponse
    {
        public T Detail { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResultResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public ResultStatus Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResultStatus
    {
        /// <summary>
        /// 接口状态
        /// </summary>
        public int Code { get; set; } = -1;
        /// <summary>
        /// 提示信息[可选填项]（缺省可以null或空字符串，甚至不包含该字段）
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 服务端计算时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
