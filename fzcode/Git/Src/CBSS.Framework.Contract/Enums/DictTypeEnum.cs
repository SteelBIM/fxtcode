using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 字典配置类型
    /// </summary>
    public enum ConfigTypeEnum
    {
        [System.ComponentModel.Description("后台菜单")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.AdminMenuConfig”的 XML 注释
        AdminMenuConfig=1,        
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.AdminMenuConfig”的 XML 注释
        [System.ComponentModel.Description("缓存")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.CacheConfig”的 XML 注释
        CacheConfig=2,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.CacheConfig”的 XML 注释
        [System.ComponentModel.Description("数据库")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.DaoConfig”的 XML 注释
        DaoConfig=3,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.DaoConfig”的 XML 注释
        [System.ComponentModel.Description("日志")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.Log4net”的 XML 注释
        Log4net=4,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.Log4net”的 XML 注释
        [System.ComponentModel.Description("业务变量")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.SettingConfig”的 XML 注释
        SettingConfig=5,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.SettingConfig”的 XML 注释
        [System.ComponentModel.Description("系统变量")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.SystemConfig”的 XML 注释
        SystemConfig=6,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ConfigTypeEnum.SystemConfig”的 XML 注释
    }
}
