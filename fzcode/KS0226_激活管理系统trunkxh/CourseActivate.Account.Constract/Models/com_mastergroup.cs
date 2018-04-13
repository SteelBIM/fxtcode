using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.Models
{
    /// <summary>
    /// 用户对应角色
    /// </summary>
    public class com_mastergroup
    {
        public int id { get; set; }
        public string mastername { get; set; }
        public int groupid { get; set; }
        public string createname { get; set; }
        public string createtime { get; set; }
    }
}
