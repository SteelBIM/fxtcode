using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.BLL
{
    public class BookBLL
    {
        BookDAL bookDAL = new BookDAL();
        /// <summary>
        /// 获取指定教材
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public QTb_Book GetBook(int bookid)
        {
            return bookDAL.GetBook(bookid);
        }
        public IList<QTb_Book> GetBookList(string strWhere, string orderby = "")
        {
            IList<QTb_Book> list = bookDAL.GetBookList(strWhere, orderby);
            return list == null ? (new List<QTb_Book>()) : list;
        }
        public IList<V_Book> GetVBookList(string strWhere, string orderby = "")
        {
            IList<V_Book> list = bookDAL.GetVBookList(strWhere, orderby);
            return list == null ? (new List<V_Book>()) : list;
        }
        /// <summary>
        /// 分页查询教材列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public IList<V_Book> GetBookPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            IList<V_Book> list=bookDAL.GetBookPageList(pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
            return list == null ? (new List<V_Book>()) : list;
        }
        /// <summary>
        /// 更新教材
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool UpdateBook(QTb_Book book)
        {
            return bookDAL.UpdateBook(book);
        }
    }
}
