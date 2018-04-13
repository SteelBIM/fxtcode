using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_resource
    {
        public int? ResID { get; set; }
        public string ResName { get; set; }
        public string ResUrl { get; set; }
        public string ResMD5 { get; set; }
        public string ResVersion { get; set; }
        public string ResKey { get; set; }
        public int? BookID { get; set; }
        public int? ModularID { get; set; }
        public string ModularName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? IsForce { get; set; }

    }
}
