using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBSLearnReport
{
    /// <summary>
    /// 学习报告学生关系变更消息队列
    /// </summary>
    public class StudentClassRelationKey
    {
        public int UserID { get; set; }

        public string ClassID { get; set; }

        /// <summary>
        /// 1-绑定  2-解绑
        /// </summary>
        public int type { get; set; }
    }
}
