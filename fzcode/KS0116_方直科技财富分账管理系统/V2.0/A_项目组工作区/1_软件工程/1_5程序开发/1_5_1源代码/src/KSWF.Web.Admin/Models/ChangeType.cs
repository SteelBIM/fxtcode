
using System;
using System.ComponentModel;
using System.Reflection;

namespace KSWF.Web.Admin.Models
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum ChangeType
    {
        //变更类型 1: 部门变更;2: 角色变更;3: 区域变更;4: 策略变更
        [Description("部门变更")]
        部门变更 = 1,
        [Description("角色变更")]
        角色变更 = 2,
        [Description("区域变更")]
        区域变更 = 3,
        [Description("策略变更")]
        策略变更 = 4
    }
}