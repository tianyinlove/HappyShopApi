using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Domian
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// 文本
        /// </summary>
        text,

        /// <summary>
        /// 图片
        /// </summary>
        image,

        /// <summary>
        /// 语音
        /// </summary>
        voice,

        /// <summary>
        /// 视频
        /// </summary>
        video,

        /// <summary>
        /// 文件
        /// </summary>
        file,

        /// <summary>
        /// 文本卡片
        /// </summary>
        textcard,

        /// <summary>
        /// 图文
        /// </summary>
        news
    }
}