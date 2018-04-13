using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum”的 XML 注释
    public enum SubjectTypeEnum:long
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum”的 XML 注释
    {
        [System.ComponentModel.Description("语文")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.Chinese”的 XML 注释
        Chinese = 167070462252163074,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.Chinese”的 XML 注释
        [System.ComponentModel.Description("数学")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.Math”的 XML 注释
        Math = 167070462323462146,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.Math”的 XML 注释
        [System.ComponentModel.Description("英语")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.English”的 XML 注释
        English = 167070462398963715
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SubjectTypeEnum.English”的 XML 注释
    }
}
