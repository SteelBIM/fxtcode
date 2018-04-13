using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// API接口请求方式
    /// </summary>
    public enum ApiFuncWayEnum
    {
        [System.ComponentModel.Description("正常")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Nornal”的 XML 注释
        Nornal = 0,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Nornal”的 XML 注释
        [System.ComponentModel.Description("加密")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Encrypt”的 XML 注释
        Encrypt = 1,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Encrypt”的 XML 注释
        [System.ComponentModel.Description("压缩")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Compress”的 XML 注释
        Compress = 2,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.Compress”的 XML 注释
        [System.ComponentModel.Description("压缩加密")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.CompressEncrypt”的 XML 注释
        CompressEncrypt = 3,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApiFuncWayEnum.CompressEncrypt”的 XML 注释

    }
}
