using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class order_psetbonus
    {
        public Guid? guid { get; set; }

        public Guid? op_guid { get; set; }

        public decimal divided { get; set; }

        public decimal class_divided { get; set; }

        public decimal p_bonus { get; set; }

        public decimal p_class_bonus { get; set; }

        public decimal p_price { get; set; }

        public string agentid { get; set; }

        public int mastertype { get; set; }
    }
}
