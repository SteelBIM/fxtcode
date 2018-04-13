using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.Contract.DataModel
{
    public class ApiFunctionRequest : Request
    {
        public string ApiFunctionName { get; set; }
        public int SystemCode { get; set; }
    }
    public class Log4netRequest : Request
    {
        public string Level { get; set; }
        public string Logger { get; set; }
    }
}
