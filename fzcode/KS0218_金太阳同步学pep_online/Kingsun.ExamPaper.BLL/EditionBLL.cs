using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.BLL
{
    public class EditionBLL
    {
        public IList<QTb_Edition> Search(string strWhere, string orderby = "")
        {
            IList<QTb_Edition> list=new EditionDAL().Search(strWhere, orderby);
            return list == null ? (new List<QTb_Edition>()) : list;
        }
    }
}
