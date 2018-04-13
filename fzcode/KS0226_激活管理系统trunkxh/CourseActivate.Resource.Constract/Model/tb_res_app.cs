using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_app
    {
        public Guid APPID { get; set; }

        public string APPName { get; set; }

        public int Type { get; set; }

        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
