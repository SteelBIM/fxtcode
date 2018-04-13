using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract.IBSResource
{
    [Auditable]
    [Table("MODBookCatalog")]
    public class MODBookCatalog
    {
        public int MODBookCatalogID { get; set; }

        public string MODBookCatalogName { get; set; }

        public string MODBookCatalogCover { get; set; }

        public int MODBookCatalogLevel { get; set; }

        public int MODBookID { get; set; }

        public int ParentId { get; set; }

        public int  PageStart{ get; set; }

        public int pageEnd { get; set; }
    }
}
