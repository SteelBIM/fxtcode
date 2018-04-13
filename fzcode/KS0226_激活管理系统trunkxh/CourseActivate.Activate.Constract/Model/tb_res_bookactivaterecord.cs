using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_res_bookactivaterecord
    {
        public int bookrecordid { get; set; }
        public int? bookid { get; set; }
        public int? num { get; set; }
        public int? usenum { get; set; }
        public DateTime? createtime { get; set; }
    }
}
