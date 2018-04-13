using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class ViewProvinceCity
    {
        public  int code { get; set; }

        public ProvinceCity data { get; set; }
    }

    public class ProvinceCity
    {
        public string province { get; set; }

        public string city { get; set; }

        public string sp { get; set; }
    }
}