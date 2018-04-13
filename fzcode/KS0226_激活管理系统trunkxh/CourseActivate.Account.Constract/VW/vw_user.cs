using CourseActivate.Account.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    public class vw_user : com_master
    {
        public string groupname { get; set; }
        public string deptname { get; set; }
        /// <summary>
        /// 负责区域
        /// </summary>
        public string responsiblearea { get; set; }
        /// <summary>
        /// 生效策略
        /// </summary>
        public string rffectivePolicy { get; set; }
        public string notrffectivePolicy { get; set; }
        /// <summary>
        /// 最后操作时间
        /// </summary>
        public string lastoperationtime { get; set; }
        /// <summary>
        /// 负责区域数
        /// </summary>
        public int areanumber { get; set; }
    }
}
