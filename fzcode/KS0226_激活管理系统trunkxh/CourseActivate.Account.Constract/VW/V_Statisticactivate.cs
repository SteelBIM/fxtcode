using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    public class V_Statisticactivate
    {
        public int bookid { get; set; }
        public int batchid { get; set; }
        public string batchcode { get; set; }
        public string activatecode { get; set; }
        public Guid activateuseid { get; set; }
        public string username { get; set; }
        public int usenum { get; set; }
        public DateTime createtime { get; set; }
    }
}
