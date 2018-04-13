using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_AppMarketBook : MarketBook
    {
        public string AppID { get; set; }

        public int Sort { get; set; }
    }
}
