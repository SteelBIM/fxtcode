using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class tb_batchactivateuse
    {

        public Guid? activateuseid { get; set; }
        public int? activateid { get; set; }
 
        public string userid { get; set; }
        public string username { get; set; }
        public DateTime? createtime { get; set; }
        public int? bookid { get; set; }
        public string activatecode { get; set; }
    }

    public class tb_batchactivateuse_copy
    {

        public Guid activateuseid { get; set; }
        public int activateid { get; set; }
        public string activatecode { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public int bookid { get; set; }
        public DateTime createtime { get; set; }
 
       
    }
}
