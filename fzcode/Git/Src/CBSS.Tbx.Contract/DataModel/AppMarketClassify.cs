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
    /// 应用分类
    /// </summary>
    [Auditable]
    [Table("AppMarketClassify")]
    public class AppMarketClassify
    {
        public int AppMarketClassifyID { get; set; }
        public string AppID { get; set; }
        public int MarketClassifyID { get; set; }  
        public int ParentId { get; set; }
        public int Sort { get; set; }
    }
}
