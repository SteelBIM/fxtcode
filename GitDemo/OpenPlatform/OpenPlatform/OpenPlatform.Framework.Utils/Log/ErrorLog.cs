using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Utils.Log
{
   public class ErrorLog
    {
       public string Url { get; set; }
       public string Ip { get; set; }
       public string UserId { get; set; }
       public int CityId { get; set; }
       public int FxtCompanyId { get; set; }
       public Exception Exception { get; set; }
       public string CustomError { get; set; }
    }
}
