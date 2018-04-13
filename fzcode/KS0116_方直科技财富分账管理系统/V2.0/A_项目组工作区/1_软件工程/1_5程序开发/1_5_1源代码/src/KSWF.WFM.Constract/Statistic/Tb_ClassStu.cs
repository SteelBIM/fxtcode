using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Statistic
{
    public class tb_classstu
    {
        public string ID { get; set; }
        public string ClassID { get; set; }
        public string StuUserID { get; set; }
        public string StuUserName { get; set; }
        public string StuTrueName { get; set; }
        public string StuMobile { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
