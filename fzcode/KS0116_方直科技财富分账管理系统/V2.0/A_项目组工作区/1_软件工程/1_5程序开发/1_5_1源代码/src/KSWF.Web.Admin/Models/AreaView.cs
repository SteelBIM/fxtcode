using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class AreaView
    {
        
        public string ID { get; set; }
        public string Seq { get; set; }
        public string ParentID { get; set; }

        public string Path { get; set; }

        public string IsEnd { get; set; }

        public string CodeName { get; set; }
    }
}