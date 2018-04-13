
using System;
using System.ComponentModel;
using System.Reflection;
namespace CourseActivate.Web.Admin.Models
{
    /// <summary>
    /// 数据操作权限 
    /// </summary>
    public enum Dataauthority
    {
        [Description("全部")]
        全部 = 0,
        [Description("本人")]
        本人 = 1,
        [Description("本人+下级部门(含本部门)+下级代理商")]
        本人本部门下级部门下级代理商 = 2,
        [Description("本人+下级部门(不含本部门)+下级代理商")]
        本人下级部门下级代理商 = 3,
        [Description("本人+下级代理商")]
        本人下级代理商 = 4
    }
}