using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 系统编号
    /// </summary>
    public enum SystemCodeEnum
    {
        [System.ComponentModel.Description("总系统后台")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.CfgmanagerAdmin”的 XML 注释
        CfgmanagerAdmin=101,        
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.CfgmanagerAdmin”的 XML 注释
        [System.ComponentModel.Description("日志后台")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.LogAdmin”的 XML 注释
        LogAdmin=102,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.LogAdmin”的 XML 注释
        [System.ComponentModel.Description("账号后台")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.AccountAdmin”的 XML 注释
        AccountAdmin=103,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.AccountAdmin”的 XML 注释
        [System.ComponentModel.Description("IBS后台")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.IBSAdmin”的 XML 注释
        IBSAdmin=104,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.IBSAdmin”的 XML 注释
        [System.ComponentModel.Description("同步学后台")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxAdmin”的 XML 注释
        TbxAdmin=105,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxAdmin”的 XML 注释
        [System.ComponentModel.Description("同步学-账号接口")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxAccountAPI”的 XML 注释
        TbxAccountAPI = 201,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxAccountAPI”的 XML 注释
        [System.ComponentModel.Description("同步学-资源接口")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxSourceAPI”的 XML 注释
        TbxSourceAPI = 202,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxSourceAPI”的 XML 注释
        [System.ComponentModel.Description("优课平台接口")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.YkptAPI”的 XML 注释
        YkptAPI=203,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.YkptAPI”的 XML 注释
        [System.ComponentModel.Description("同步学服务")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxFS”的 XML 注释
        TbxFS=301,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxFS”的 XML 注释
        [System.ComponentModel.Description("同步学-支付接口")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxPayAPI”的 XML 注释
        TbxPayAPI = 204,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SystemCodeEnum.TbxPayAPI”的 XML 注释
    }
}
