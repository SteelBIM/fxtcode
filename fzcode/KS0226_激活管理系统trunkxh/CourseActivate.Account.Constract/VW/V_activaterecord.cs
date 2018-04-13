using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    public class V_activaterecord
    {
        public int bookid { get; set; }
        public string activatecode { get; set; }
        public string username { get; set; }
        public string devicecode { get; set; }
        public int? devicetype { get; set; }
        public int? isios { get; set; }
        public DateTime createtime { get; set; }
        public string BookName { get; set; }
    }
}
