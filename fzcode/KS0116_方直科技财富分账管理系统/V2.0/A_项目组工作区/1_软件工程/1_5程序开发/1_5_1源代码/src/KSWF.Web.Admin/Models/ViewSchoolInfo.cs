using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class ViewSchoolInfo
    {
        public int ID { get; set; }
        public string SchoolName { get; set; }
        public string DistrictID { get; set; }
        public string TownsID { get; set; }
        public string Area { get; set; }

        public string SchoolTypeNo { get; set; }
    }
}