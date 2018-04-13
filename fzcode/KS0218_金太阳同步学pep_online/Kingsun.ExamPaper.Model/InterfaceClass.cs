using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.ExamPaper.Model
{
    class InterfaceClass
    {
    }

    public class CatalogInfo
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        public int BookID { get; set; }

    }

    public class tbStuCatalog
    {
        public string StuCatID { get; set; }
        public int AnswerNum { get; set; }
    }
}
