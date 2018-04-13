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
    [Table("Sys_ApiFunctionParam")]
    public class Sys_ApiFunctionParam
    {
        public int ApiFunctionParamID { get; set; }
        public string ApiFunctionParamParentID { get; set; }
        public string ParameterFields { get; set; }
        public string ParameterExplain { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterType { get; set; }
        public int IsAllowNull { get; set; }
        public DateTime CreateDate { get; set; }
        public int ApiFunctionID { get; set; }
        public int Type { get; set; }
    }
}
