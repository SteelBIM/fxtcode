using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.Admin.Models
{
    public class CourseCondition
    {
        public int Period { get; set; }
        public int Grade { get; set; }
        public int Edition { get; set; }
        public int Subject { get; set; }
        public int Reel { get; set; }
        public int Status { get; set; }

        public int Publish { get; set; }
    }
}