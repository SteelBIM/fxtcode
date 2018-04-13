using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CBSS.Core.Utility.Models
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>”的 XML 注释
    public class XmlNodeComponent<TValue>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.Node”的 XML 注释
        public XmlNode Node { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.Node”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.Value”的 XML 注释
        public TValue Value { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.Value”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.HasNode”的 XML 注释
        public bool HasNode
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.HasNode”的 XML 注释
        {
            get
            {
                return Node != null;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.HasInnerText”的 XML 注释
        public bool HasInnerText
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XmlNodeComponent<TValue>.HasInnerText”的 XML 注释
        {
            get
            {
                return Node != null && !string.IsNullOrWhiteSpace(Node.InnerText);
            }
        }
    }
}
