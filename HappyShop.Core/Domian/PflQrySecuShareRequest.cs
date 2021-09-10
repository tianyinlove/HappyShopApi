using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyShop.Domian
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoBuf.ProtoContract(Name = @"PflQrySecuShare_Request")]
    public class PflQrySecuShareRequest
    {
        /// <summary>
        /// 登录凭据(服务端之间不使用)
        /// </summary>
        [ProtoBuf.ProtoMember(1, Name = @"token")]
        [JsonProperty("token")]
        [System.ComponentModel.DefaultValue("")]
        public string Token { get; set; } = "";

        /// <summary>
        /// 锦囊id
        /// </summary>
        [ProtoBuf.ProtoMember(2, Name = @"prodid", DataFormat = ProtoBuf.DataFormat.FixedSize)]
        [JsonProperty("prodid")]
        public uint Prodid { get; set; }

        /// <summary>
        /// 市场代码：0:全部；1:深圳；2:上海
        /// </summary>
        [ProtoBuf.ProtoMember(3, Name = @"market")]
        [JsonProperty("market")]
        public int Market { get; set; }

        /// <summary>
        /// 证券代码：空则查全部
        /// </summary>
        [ProtoBuf.ProtoMember(4, Name = @"secucode")]
        [JsonProperty("secucode")]
        [System.ComponentModel.DefaultValue("")]
        public string Secucode { get; set; } = "";

        /// <summary>
        /// 分页开始书签：
        /// </summary>
        [ProtoBuf.ProtoMember(5, Name = @"pos")]
        [JsonProperty("pos")]
        public uint Pos { get; set; }

        /// <summary>
        /// 分页请求数量：
        /// </summary>
        [ProtoBuf.ProtoMember(6, Name = @"req")]
        [JsonProperty("req")]
        public uint Req { get; set; }

        /// <summary>
        /// 带市场股票代码 （和market、secucode 二选一）
        /// </summary>
        [ProtoBuf.ProtoMember(7, Name = @"stockcode")]
        [JsonProperty("stockcode")]
        public uint Stockcode { get; set; }

    }
}
