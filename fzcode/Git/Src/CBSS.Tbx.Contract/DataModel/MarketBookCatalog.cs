using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("MarketBookCatalog")]
    public class MarketBookCatalog
    {
        public int MarketBookCatalogID { get; set; }

        public int? MarketBookID { get; set; }

        public string MarketBookCatalogName { get; set; }

        public string MarketBookCatalogCover { get; set; }

        public int? MODBookCatalogID { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateUser { get; set; }

        public int? ParentCatalogID { get; set; }

        public string MODBookCatalogName { get; set; }

        public string MODBookCatalogCover { get; set; }
        /// <summary>
        /// 显示or隐藏
        /// </summary>
        public int? IsShow { get; set; }

        public int? StartPage { get; set; }

        public int? EndPage { get; set; }

    }
}
