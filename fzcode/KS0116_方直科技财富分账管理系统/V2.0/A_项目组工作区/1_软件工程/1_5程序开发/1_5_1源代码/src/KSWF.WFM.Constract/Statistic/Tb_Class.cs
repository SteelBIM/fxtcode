using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Statistic
{
    public class tb_class
    {
        public string ID { get; set; }
        public string ClassName { get; set; }
        public int? SchoolID { get; set; }
        public string SchoolName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Flag { get; set; }
        public string ClassNum { get; set; }
        public int? GradeID { get; set; }
        public string GradeName { get; set; }
    }
}
