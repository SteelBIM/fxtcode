using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class masterbpolicypr
    {
        public string effectivestatus { get; set; }
        public int? id { get; set; }
        public int? bid { get; set; }
        public DateTime? startdate { get; set; }
        public string mastername { get; set; }
        public int? pid { get; set; }
        public string pllicyname { get; set; }
        public string productname { get; set; }
        public DateTime? createtime { get; set; }
        public int? versionid { get; set; }
        public string version { get; set; }
        public string category { get; set; }
        public int? categorykey { get; set; }
        public decimal? divided { get; set; }
        public decimal? class_divided { get; set; }
    }
}