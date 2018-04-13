using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.API
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum”的 XML 注释
    public enum ModErrorCodeEnum
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.正常”的 XML 注释
        正常=0,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.正常”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.编解码错误”的 XML 注释
        编解码错误 =10000,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.编解码错误”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.参数错误”的 XML 注释
        参数错误 = 100001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.参数错误”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.请求包不完整”的 XML 注释
        请求包不完整 = 100002,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.请求包不完整”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.错误的命令”的 XML 注释
        错误的命令 = 100003,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModErrorCodeEnum.错误的命令”的 XML 注释
    }
}
