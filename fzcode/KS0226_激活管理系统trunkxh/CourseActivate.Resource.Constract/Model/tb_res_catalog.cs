using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_catalog
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        public int? BookID { get; set; }
        public string ModularIDS { get; set; }
        public int? CatalogLevel { get; set; }
        public int? PageNoStart { get; set; }
        public int? PageNoEnd { get; set; }
        public int ParentID { get; set; }
        public int? Sort { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
