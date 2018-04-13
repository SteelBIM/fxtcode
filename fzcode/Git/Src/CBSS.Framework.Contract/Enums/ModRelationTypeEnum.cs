using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// MOD关系类别编号
    /// </summary>
    public enum ModRelationTypeEnum
    {
        /// <summary>
        /// 教师和学生
        /// </summary>
        [System.ComponentModel.Description("教师和学生")]
        TeaStu =1,
        /// <summary>
        /// 家长和学生
        /// </summary>
        [System.ComponentModel.Description("家长和学生")]
        ParentStu =2,
        /// <summary>
        /// 业务员和代理商
        /// </summary>
        [System.ComponentModel.Description("业务员和代理商")]
        SalesAgent =3,
        /// <summary>
        /// 代理商和教师
        /// </summary>
        [System.ComponentModel.Description("代理商和教师")]
        AgentTea =4,
        /// <summary>
        /// 学生和班级
        /// </summary>
        [StringValue("学生和班级")]
        [System.ComponentModel.Description("学生和班级")]
        StuClass =5,
        /// <summary>
        /// 教师和班级
        /// </summary>
        [System.ComponentModel.Description("教师和班级")]
        TeaClass =6
    }
}
