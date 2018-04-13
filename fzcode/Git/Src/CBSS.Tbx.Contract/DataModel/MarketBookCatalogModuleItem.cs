using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class AppBookCatalogModuleItem
    {
        public int AppBookCatalogModuleItemID { get; set; }

        public Guid AppID { get; set; }

        public int MarketBookCatalogID { get; set; }

        public int ModuleID { get; set; }

        

        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }

        public int CreateUser { get; set; }

        public string BeforeBuyingImg { get; set; }
        public string ModuleName { get; set; }

        public string BuyLaterImg { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }
        public string BeforeBuyingClickImg { get; set; }
        public string BuyLaterClickImg { get; set; }
    }
}
