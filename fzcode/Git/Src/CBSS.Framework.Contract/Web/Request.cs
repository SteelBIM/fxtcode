using System;
using System.Collections.Generic;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 用于BLL方法提传入条件
    /// </summary>
    public class Request : ModelBase
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Request.Request()”的 XML 注释
        public Request()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Request.Request()”的 XML 注释
        {
            PageSize = 5000;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Request.Top”的 XML 注释
        public int Top
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Request.Top”的 XML 注释
        {
            set
            {
                this.PageSize = value;
                this.PageIndex = 1;
            }
        }
        
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Request.PageSize”的 XML 注释
        public int PageSize { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Request.PageSize”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Request.PageIndex”的 XML 注释
        public int PageIndex { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Request.PageIndex”的 XML 注释
    }
}
