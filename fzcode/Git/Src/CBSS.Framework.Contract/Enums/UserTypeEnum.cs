using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 用户身份
    /// </summary>
    public enum UserTypeEnum
    {
        [System.ComponentModel.Description("普通身份")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Nornal”的 XML 注释
        Nornal = 1,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Nornal”的 XML 注释
        [System.ComponentModel.Description("教师身份")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Teacher”的 XML 注释
        Teacher = 12,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Teacher”的 XML 注释
        [System.ComponentModel.Description("学生身份")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Student”的 XML 注释
        Student = 26,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Student”的 XML 注释
        [System.ComponentModel.Description("家长身份")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Parent”的 XML 注释
        Parent = 27,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserTypeEnum.Parent”的 XML 注释

    }
}
