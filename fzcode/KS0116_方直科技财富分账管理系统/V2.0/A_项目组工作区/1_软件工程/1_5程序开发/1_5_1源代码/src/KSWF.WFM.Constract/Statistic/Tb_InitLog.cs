using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Statistic
{
    public class tb_initlog
    {
        public int ID { get; set; }
        public DateTime InitDate { get; set; }
        public string InitTable { get; set; }
        public int InitDataCount { get; set; }
        public string InitMaster { get; set; }

       
    }
}
