using HappyShop.Domian;
using HappyShop.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Service
{
    /// <summary>
    ///
    /// </summary>
    public interface IWeChatService
    {
        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="request">通知对象</param>
        /// <returns></returns>
        Task<bool> Notice(WechatMessageRequest request);
    }
}