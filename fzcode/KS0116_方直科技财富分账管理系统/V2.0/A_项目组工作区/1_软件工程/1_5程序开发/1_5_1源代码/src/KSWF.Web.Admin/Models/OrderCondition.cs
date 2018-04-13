using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class OrderCondition
    {
        public int SearchType { get; set; }
        public string SearchKey { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? PayType { get; set; }
        public int? AreaCode { get; set; }
        public int? SchoolID { get; set; }
        public int? GradeID { get; set; }
        public string ClassID { get; set; }

        public int? ChannelID { get; set; }
        public int? Category { get; set; }

        public int? Dept { get; set; }
        public string Qudao { get; set; }
        public string MasterName { get; set; }
        public string Agency { get; set; }
        public int? Channel { get; set; }

        public  int? Version { get; set; }

    }
}