using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class Tb_PhoneCode
    {
        public int ID { get; set; }
        
        public string TelePhone { get; set; }
        
        public string Code { get; set; }
        
        public DateTime? EndDate { get; set; }

    }
}
