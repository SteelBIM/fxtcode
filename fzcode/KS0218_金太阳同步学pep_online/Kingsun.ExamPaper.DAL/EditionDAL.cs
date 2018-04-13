using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.DAL
{
    public class EditionDAL : BaseManagement
    {
        public IList<QTb_Edition> Search(string strWhere, string orderby = "")
        {
            return Search<QTb_Edition>(strWhere, orderby);
        }
    }
}
