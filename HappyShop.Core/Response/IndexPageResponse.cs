using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyShop.Response
{
    /// <summary>
    ///
    /// </summary>
    public class IndexPageResponse<T>
    {
        /// <summary>
        /// 普通列表
        /// </summary>
        public List<T> List { get; set; } = new List<T>();

        /// <summary>
        /// 当前页码(从0开始)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 翻页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 最大页（从0开始）
        /// </summary>
        public int MaxPage
        {
            get
            {
                if (Total <= 0 || PageSize <= 0)
                {
                    return 0;
                }

                if (Total % PageSize == 0)
                {
                    return Total / PageSize - 1;
                }

                return Total / PageSize;
            }
        }
    }
}