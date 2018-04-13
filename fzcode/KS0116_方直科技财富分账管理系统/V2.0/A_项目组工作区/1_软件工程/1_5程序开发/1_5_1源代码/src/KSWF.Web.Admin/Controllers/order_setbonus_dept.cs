using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSWF.Web.Admin.Controllers
{
    public class order_setbonus_dept
    {
        public Guid? osid { get; set; }
        public string os_no { get; set; }
        public int deptid { get; set; }
        public string deptname { get; set; }
        public string mastername_t { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public int total_count { get; set; }
        public decimal? total_amount { get; set; }
        public decimal? total_bonus { get; set; }
        public decimal? adjust_amount { get; set; }
        public string adjust_reason { get; set; }
        public decimal? team_bonus_r { get; set; }
        public int state { get; set; }
        public string mastername { get; set; }
        public DateTime createtime { get; set; }
        public string agentid { get; set; }
    }
}
