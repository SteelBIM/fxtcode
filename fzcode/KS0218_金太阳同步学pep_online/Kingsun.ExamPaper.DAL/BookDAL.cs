using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.DAL
{
    public class BookDAL : BaseManagement
    {
        public QTb_Book GetBook(int bookid)
        {
            return Select<QTb_Book>(bookid);
        }
        public IList<QTb_Book> GetBookList(string strWhere, string orderby = "")
        {
            return Search<QTb_Book>(strWhere, orderby);
        }
        public IList<V_Book> GetVBookList(string strWhere, string orderby = "")
        {
            return Search<V_Book>(strWhere, orderby);
        }

        public IList<V_Book> GetBookPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            return GetPageList<V_Book>("V_Book", pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
        }

        public bool UpdateBook(QTb_Book book)
        {
            return Update<QTb_Book>(book);
        }
    }
}
