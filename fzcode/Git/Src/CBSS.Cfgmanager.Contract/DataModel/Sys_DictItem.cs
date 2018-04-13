using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.Contract.DataModel
{
    public class Sys_DictItem
    {
        public int DictItemID { get; set; }
        public string DictCode { get; set; }
        public string DictName { get; set; }
        public string DictValue { get; set; }
        public int DictSort { get; set; }

        public int SystemCode { get; set; }
        public int ConfigType { get; set; }
        public int ParentID { get; set; }
        public int State { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
    }
}
