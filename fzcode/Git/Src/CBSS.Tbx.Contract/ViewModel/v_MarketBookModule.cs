using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_MarketBookModule
    {
        public int MarketBookID { get; set; }
        public int MarketClassifyId { get; set; }
        public string MarketBookName { get; set; }
        public string MarketClassifyName { get; set; }
        public string ModuleNames { get; set; }
        public string MODBookCover { get; set; }
        public string MODBookName { get; set; }
        public string MarketBookCover { get; set; }
        public int MODID { get; set; }
        public DateTime CreateDate { get; set; }
        public string ISBN { get; set; }
    }
}
