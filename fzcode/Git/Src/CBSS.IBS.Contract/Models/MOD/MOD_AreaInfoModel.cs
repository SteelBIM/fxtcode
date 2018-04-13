using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBSS.IBS.Contract
{
    public class MOD_AreaInfoModel
    {
        public string ID { get; set; }
        public int ParentID { get; set; }
        public string CodeName { get; set; }
    }
    public class MOD_AreaDataModel
    {

        public int ParentID { get; set; }

        public long ID { get; set; }

        public string CodeName { get; set; }
    }
}
