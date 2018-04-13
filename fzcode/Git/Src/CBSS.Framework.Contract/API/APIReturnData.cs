using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract.API
{
    /// <summary>
    /// 接口方法返回结果
    /// </summary>
    public class APIReturnData
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“APIReturnData.Key”的 XML 注释
        public string Key { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“APIReturnData.Key”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“APIReturnData.Info”的 XML 注释
        public string Info { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“APIReturnData.Info”的 XML 注释
    }
}
