using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("Sys_ApiFunction")]
    public class Sys_ApiFunction
    {
        public int ApiFunctionID { get; set; }
        public string ApiFunctionName { get; set; }
        public string ApiFunctionExplain { get; set; }
        public string ApiFunctionUrl { get; set; }
        public int ApiFunctionWay { get; set; }
        public string ApiFunctionRemark { get; set; }
        public string SystemCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int State { get; set; }
    }
}
