using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Utility.Extensions;

namespace HappyShop.Comm
{
    public class GatewayData
    {
        #region 私有字段

        readonly SortedDictionary<string, object> _values;

        #endregion 私有字段

        #region 属性

        public object this[string key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        public int Count => _values.Count;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public GatewayData()
        {
            _values = new SortedDictionary<string, object>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="comparer">排序策略</param>
        public GatewayData(IComparer<string> comparer)
        {
            _values = new SortedDictionary<string, object>(comparer);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">1:json;2:xml;3:urlparam</param>
        /// <param name="data">数据</param>
        public GatewayData(DataFormat type, string data)
        {
            _values = new SortedDictionary<string, object>();
            if (type == DataFormat.Json)
            {
                FromJson(data);
            }
            else if (type == DataFormat.Xml)
            {
                FromXml(data);
            }
            else if (type == DataFormat.Url)
            {
                FromUrl(data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="type">1:json;2:xml;3:urlparam</param>
        /// <param name="data">数据</param>
        public GatewayData(IComparer<string> comparer, int type, string data)
        {
            _values = new SortedDictionary<string, object>(comparer);
            if (type == 1)
            {
                FromJson(data);
            }
            else if (type == 2)
            {
                FromXml(data);
            }
            else if (type == 3)
            {
                FromUrl(data);
            }
        }

        #endregion 构造函数

        #region 公开方法

        /// <summary>
        /// 将Url格式数据转换为网关数据
        /// </summary>
        /// <param name="url">url数据</param>
        /// <param name="isUrlDecode">是否需要url解码</param>
        /// <returns></returns>
        public void FromUrl(string url, bool isUrlDecode = true)
        {
            try
            {
                Clear();
                if (!string.IsNullOrEmpty(url))
                {
                    int index = url.IndexOf('?');

                    if (index == 0)
                    {
                        url = url.Substring(index + 1);
                    }

                    var regex = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
                    var mc = regex.Matches(url);

                    foreach (Match item in mc)
                    {
                        string value = item.Result("$3");
                        Add(item.Result("$2"), isUrlDecode ? WebUtility.UrlDecode(value) : value);
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 将Xml格式数据转换为网关数据
        /// </summary>
        /// <param name="xml">Xml数据</param>
        /// <returns></returns>
        public void FromXml(string xml)
        {
            try
            {
                Clear();
                if (!string.IsNullOrEmpty(xml))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    var xmlNode = xmlDoc.FirstChild;
                    var nodes = xmlNode.ChildNodes;
                    foreach (var item in nodes)
                    {
                        var xe = (XmlElement)item;
                        if (!xe.IsEmpty && !string.IsNullOrEmpty(xe.InnerText))
                        {
                            Add(xe.Name, xe.InnerText);
                        }
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 将Json格式数据转成网关数据
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public void FromJson(string json)
        {
            try
            {
                Clear();
                if (!string.IsNullOrEmpty(json))
                {
                    var jObject = JObject.Parse(json);
                    var list = jObject.Children().OfType<JProperty>();
                    foreach (var item in list)
                    {
                        Add(item.Name, item.Value.ToString());
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 是否存在指定参数名
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public bool Exists(string key) => _values.ContainsKey(key);

        /// <summary>
        /// 清空网关数据
        /// </summary>
        public void Clear()
        {
            _values.Clear();
        }

        /// <summary>
        /// 移除指定参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _values.Remove(key);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }

            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            if (Exists(key))
            {
                _values[key] = value;
            }
            else
            {
                _values.Add(key, value);
            }

            return true;
        }


        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="stringCase">字符串策略</param>
        /// <returns></returns>
        public bool Add(object obj, StringCase stringCase)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();
            var fields = type.GetFields();

            Add(properties);
            Add(fields);

            return true;

            void Add(MemberInfo[] info)
            {
                foreach (var item in info)
                {
                    var notAddattributes = item.GetCustomAttributes(typeof(NotAddAttribute), true);
                    if (notAddattributes.Length > 0)
                    {
                        continue;
                    }

                    string key;
                    object value;
                    var renameAttribute = item.GetCustomAttributes(typeof(ReNameAttribute), true);
                    if (renameAttribute.Length > 0)
                    {
                        key = ((ReNameAttribute)renameAttribute[0]).Name;
                    }
                    else
                    {
                        if (stringCase is StringCase.Camel)
                        {
                            key = item.Name.ToCamelCase();
                        }
                        else if (stringCase is StringCase.Snake)
                        {
                            key = item.Name.ToSnakeCase();
                        }
                        else
                        {
                            key = item.Name;
                        }
                    }

                    switch (item.MemberType)
                    {
                        case MemberTypes.Field:
                            value = ((FieldInfo)item).GetValue(obj);
                            break;

                        case MemberTypes.Property:
                            value = ((PropertyInfo)item).GetValue(obj);
                            break;

                        default:
                            throw new NotImplementedException();
                    }

                    if (value is null || string.IsNullOrEmpty(value.ToString()))
                    {
                        continue;
                    }

                    if (Exists(key))
                    {
                        _values[key] = value;
                    }
                    else
                    {
                        _values.Add(key, value);
                    }
                }
            }
        }

        /// <summary>
        /// 根据参数名获取参数值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public object GetValue(string key)
        {
            _values.TryGetValue(key, out object value);
            return value;
        }

        /// <summary>
        /// 根据参数名获取参数值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值</returns>
        public T GetValue<T>(string key)
        {
            _values.TryGetValue(key, out object value);
            if (value != null)
            {
                return (T)(value);
            }
            return default(T);
        }

        /// <summary>
        /// 将网关参数转为类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="stringCase">字符串策略</param>
        /// <returns></returns>
        public T ToObject<T>(StringCase stringCase)
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type);
            var properties = type.GetProperties();

            foreach (var item in properties)
            {
                var renameAttribute = item.GetCustomAttributes(typeof(ReNameAttribute), true);

                string key;
                if (renameAttribute.Length > 0)
                {
                    key = ((ReNameAttribute)renameAttribute[0]).Name;
                }
                else
                {
                    if (stringCase is StringCase.Camel)
                    {
                        key = item.Name.ToCamelCase();
                    }
                    else if (stringCase is StringCase.Snake)
                    {
                        key = item.Name.ToSnakeCase();
                    }
                    else
                    {
                        key = item.Name;
                    }
                }

                var value = GetValue(key);

                if (value != null)
                {
                    item.SetValue(obj, Convert.ChangeType(value, item.PropertyType));
                }
            }

            return (T)obj;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="isUrlEncode">是否编码</param>
        /// <param name="valueQuoteEncode">是否需要转义</param>
        /// <returns>拼接完成以后的字符串</returns>
        public string ToUrl(bool isUrlEncode, bool valueQuoteEncode)
        {
            if (valueQuoteEncode)
            {
                return string.Join("&", _values.Select(a => $"{a.Key}=\"{(isUrlEncode ? WebUtility.UrlEncode(GetJson(a)) : GetJson(a))}\""));
            }
            else
            {
                return string.Join("&", _values.Select(a => $"{a.Key}={(isUrlEncode ? WebUtility.UrlEncode(GetJson(a)) : GetJson(a))}"));
            }
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="isUrlEncode">是否编码</param>
        /// <param name="valueQuoteEncode">是否需要转义</param>
        /// <returns>拼接完成以后的字符串</returns>
        public string ToUrlParam(bool isUrlEncode, bool valueQuoteEncode)
        {
            StringBuilder prestr = new StringBuilder();

            foreach (var temp in _values)
            {
                if (!valueQuoteEncode)
                    prestr.Append(temp.Key + "=" + (isUrlEncode ? WebUtility.UrlEncode(GetJson(temp)) : GetJson(temp)) + "&");
                else
                    prestr.Append(temp.Key + "=\"" + (isUrlEncode ? WebUtility.UrlEncode(GetJson(temp)) : GetJson(temp)) + "\"&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUrlEncode"></param>
        /// <returns></returns>
        public string ToJsonParam()
        {
            return _values.ToJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUrlEncode"></param>
        /// <returns></returns>
        public IDictionary<string, string> ToDictionaryParam(bool isUrlEncode)
        {
            IDictionary<string, string> paramsMap = new Dictionary<string, string>();
            foreach (var temp in _values)
            {
                paramsMap.Add(temp.Key, (isUrlEncode ? WebUtility.UrlEncode(GetJson(temp)) : GetJson(temp)));
            }
            return paramsMap;
        }

        /// <summary>
        /// 将网关数据转成Xml格式数据
        /// </summary>
        /// <returns></returns>
        public string ToXmlParam()
        {
            if (_values == null || _values.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var item in _values)
            {
                if (item.Value is string)
                {
                    sb.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", item.Key, item.Value);
                }
                else
                {
                    sb.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
                }
            }
            sb.Append("</xml>");

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetJson(KeyValuePair<string, object> data)
        {
            var type = data.Value.GetType();
            if (!IsBaseType(type))
            {
                var properties = type.GetProperties();
                return GetJsonData(data.Value, properties, StringCase.Snake);
            }
            return data.Value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="stringCase"></param>
        /// <returns></returns>
        string GetJsonData(object obj, MemberInfo[] info, StringCase stringCase)
        {
            var data = new Dictionary<string, object>();
            foreach (var item in info)
            {
                var notAddattributes = item.GetCustomAttributes(typeof(NotAddAttribute), true);
                if (notAddattributes.Length > 0)
                {
                    continue;
                }

                string key;
                object value;
                var renameAttribute = item.GetCustomAttributes(typeof(ReNameAttribute), true);
                if (renameAttribute.Length > 0)
                {
                    key = ((ReNameAttribute)renameAttribute[0]).Name;
                }
                else
                {
                    if (stringCase is StringCase.Camel)
                    {
                        key = item.Name.ToCamelCase();
                    }
                    else if (stringCase is StringCase.Snake)
                    {
                        key = item.Name.ToSnakeCase();
                    }
                    else
                    {
                        key = item.Name;
                    }
                }

                switch (item.MemberType)
                {
                    case MemberTypes.Field:
                        value = ((FieldInfo)item).GetValue(obj);
                        break;

                    case MemberTypes.Property:
                        value = ((PropertyInfo)item).GetValue(obj);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                if (value is null || string.IsNullOrEmpty(value.ToString()))
                {
                    continue;
                }
                if (data.ContainsKey(key))
                {
                    data[key] = value;
                }
                else
                {
                    data.Add(key, value);
                }
            }

            return data.ToJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool IsBaseType(Type t)
        {
            return string.Equals(t.Name, typeof(string).Name) ||
                string.Equals(t.Name, typeof(int).Name) ||
                string.Equals(t.Name, typeof(long).Name) ||
                string.Equals(t.Name, typeof(decimal).Name) ||
                string.Equals(t.Name, typeof(double).Name) ||
                string.Equals(t.Name, typeof(char).Name) ||
                string.Equals(t.Name, typeof(DateTime).Name) ||
                string.Equals(t.Name, typeof(float).Name);
        }

        #endregion
    }
}
