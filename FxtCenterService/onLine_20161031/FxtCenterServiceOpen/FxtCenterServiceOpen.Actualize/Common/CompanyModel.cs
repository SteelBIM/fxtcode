using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterServiceOpen.Actualize.Common
{
    [Serializable]
    public class CompanyModel
    {
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public int CompanyID { get; set; }
    }
}
