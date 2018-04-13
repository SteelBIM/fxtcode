using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBSS.Core.Utility;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 业务异常，用于在后端抛出到前端做相应处理
    /// </summary>
    public class BusinessException : Exception
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException()”的 XML 注释
        public BusinessException()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException()”的 XML 注释
            : this(string.Empty)
        {
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string)”的 XML 注释
        public BusinessException(string message):
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string)”的 XML 注释
            this("error", message)
        {
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, string)”的 XML 注释
        public BusinessException(string name, string message)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, string)”的 XML 注释
            :base(message)
        {
            this.Name = name;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, Enum)”的 XML 注释
        public BusinessException(string message, Enum errorCode)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, Enum)”的 XML 注释
            : this("error", message, errorCode)
        {
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, string, Enum)”的 XML 注释
        public BusinessException(string name, string message, Enum errorCode)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.BusinessException(string, string, Enum)”的 XML 注释
            : base(message)
        {
            this.Name = name;
            this.ErrorCode = errorCode;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.Name”的 XML 注释
        public string Name { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.Name”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BusinessException.ErrorCode”的 XML 注释
        public Enum ErrorCode { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BusinessException.ErrorCode”的 XML 注释
    }
}