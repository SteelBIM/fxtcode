using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    /// <summary>
    /// 应用选择书籍
    /// </summary>
    [Auditable]
    [Table("AppMarketBook")]
    public class AppMarketBook
    {
        public int AppMarketBookID { get; set; }
        public string AppID { get; set; }
        public int MarketBookID { get; set; }  
        public int MarketClassifyID { get; set; }
        public int Sort { get; set; }
    }
}
