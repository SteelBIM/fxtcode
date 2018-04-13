using System.Collections.Generic;

namespace Kingsun.ExamPaper.Model
{
    /// <summary>
    /// 自定义课本目录结构
    /// </summary>
    public class Custom_BookCatalog
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public IList<Custom_Catalog> CatalogList { get; set; }
    }
}
