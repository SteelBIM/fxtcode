using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KSWF.Web.Admin.Models
{
    public class DeptSaveView
    {
        [Required]
        public string deptname { get; set; }

        //修改时为deptid
        public int deptid { get; set; }

        public int parentid { get; set; }

        //[Required]
        public string districtids { get; set; }

        public int isend { get; set; }

        public int level { get; set; }
    }
}