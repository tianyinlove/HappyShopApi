using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Comm
{
    /// <summary>
    /// 重命名属性
    /// </summary>
    public class ReNameAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
