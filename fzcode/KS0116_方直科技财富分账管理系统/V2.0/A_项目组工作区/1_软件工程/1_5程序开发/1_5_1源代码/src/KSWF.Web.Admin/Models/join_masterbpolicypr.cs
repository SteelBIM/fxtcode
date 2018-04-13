using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class join_masterbpolicypr
    {
        public int id { get; set; }
        public string mastername { get; set; }
        //策略ID
        public int bid { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime startdate { get; set; }
 
        public int createid { get; set; }
        public DateTime createtime { get; set; }
        public string grouplogo { get; set; }
    }
}