using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class Product
    {

        public string ProductName { get; set; }
        public int? GradeID { get; set; }
        public string Grade { get; set; }
        public int? VersionID { get; set; }
        public string Version { get; set; }
        public int? SubjectID { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public int? CategoryKey { get; set; }
        public int? ID { get; set; }
        public string ProductNo { get; set; }
        public int? Channel { get; set; }

        public int Isshevel { get; set; }
        public decimal Price { get; set; }
        public int Delflg { get; set; }
    }
}