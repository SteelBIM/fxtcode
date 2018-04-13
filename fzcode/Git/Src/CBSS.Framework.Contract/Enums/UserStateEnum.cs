using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum”的 XML 注释
    public enum UserStateEnum
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum”的 XML 注释
    {
        [System.ComponentModel.Description("未登录")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum.notLogin”的 XML 注释
#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
        /// <remarks/>
        notLogin,
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum.notLogin”的 XML 注释
        [System.ComponentModel.Description("不在线")]
#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum.notOnLine”的 XML 注释
        /// <remarks/>
        notOnLine,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum.notOnLine”的 XML 注释
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
        [System.ComponentModel.Description("其他")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum.otherOnline”的 XML 注释
#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
        /// <remarks/>
        otherOnline,
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum.otherOnline”的 XML 注释
        [System.ComponentModel.Description("普通身份")]
#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum.selfOnline”的 XML 注释
        /// <remarks/>
        selfOnline,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum.selfOnline”的 XML 注释
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
        [System.ComponentModel.Description("普通身份")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserStateEnum.appForbidden”的 XML 注释
#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
        /// <remarks/>
        appForbidden,
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserStateEnum.appForbidden”的 XML 注释
    }
}
