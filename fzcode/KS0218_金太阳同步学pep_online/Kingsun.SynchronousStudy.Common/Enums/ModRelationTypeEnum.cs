using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    /// <summary>
    /// MOD关系类别编号
    /// </summary>
    public enum ModRelationTypeEnum
    {
        /// <summary>
        /// 教师和学生
        /// </summary>
        TeaStu=1,
        /// <summary>
        /// 家长和学生
        /// </summary>
        ParentStu=2,
        /// <summary>
        /// 业务员和代理商
        /// </summary>
        SalesAgent=3,
        /// <summary>
        /// 代理商和教师
        /// </summary>
        AgentTea=4,
        /// <summary>
        /// 学生和班级
        /// </summary>
        StuClass=5,
        /// <summary>
        /// 教师和班级
        /// </summary>
        TeaClass=6
    }
}
