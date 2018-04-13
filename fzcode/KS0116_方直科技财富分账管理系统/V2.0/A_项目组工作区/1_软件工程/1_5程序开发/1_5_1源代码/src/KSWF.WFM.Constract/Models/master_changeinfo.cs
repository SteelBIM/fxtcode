using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class master_changeinfo
    {
        public int id { get; set; }
        public string mastername { get; set; }
        /// <summary>
        /// 变更类型 1: 部门变更;2: 角色变更;3: 区域变更;4: 策略变更
        /// </summary>
        public int changetype { get; set; }
        public string old_id { get; set; }
        public string old_name { get; set; }
        public string new_id { get; set; }
        public string new_name { get; set; }
        public string createname { get; set; }
        public DateTime createtime { get; set; }
    }
}
