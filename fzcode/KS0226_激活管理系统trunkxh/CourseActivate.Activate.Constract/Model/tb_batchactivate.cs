using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_batchactivate
    {
        public int activateid { get; set; }
        public string activatecode { get; set; }
        public int? batchid { get; set; }     

        public int? ismatch { get; set; }

        public DateTime? createtime { get; set; }
    }

    /// <summary>
    /// 用于海量导入
    /// </summary>
    public class tb_batchactivate_copy
    {
        public int activateid { get; set; }
        public string activatecode { get; set; }
        public int batchid { get; set; }

        public int ismatch { get; set; }

        public DateTime createtime { get; set; }
    }
}
