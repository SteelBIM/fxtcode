using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_appversion
    {
        public int? APPVersionID { get; set; }
        public string Version { get; set; }
        public string Url { get; set; }
        public string MD5 { get; set; }
        public int? IsForce { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }
        public Guid? AppID { get; set; }    
        public DateTime CreateDate { get; set; }
    }
}
