using Kingsun.ExamPaper.DAL;
using Kingsun.ExamPaper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace Kingsun.ExamPaper.BLL
{
    public class CatalogBLL
    {
        CatalogDAL catalogDAL = new CatalogDAL();
        /// <summary>
        /// 获取课本及目录信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="mod_ed">mod对应版本ID</param>
        /// <param name="gradeid">年级ID</param>
        /// <param name="bookreel">册别</param>
        /// <param name="subjectid">学科（默认为英语：3）</param>
        /// <returns></returns>
        public IList<Custom_BookCatalog> GetBookAndCatalogList(int? BookID, string UserID)
        {
            DataSet ds = catalogDAL.GetBookAndCatalogList(BookID, UserID);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                IList<Custom_BookCatalog> listBC = new List<Custom_BookCatalog>();
                DataTable dt = ds.Tables[0];
                //01.查询Book(第一个参数代表distinct)
                DataTable dtBook = dt.DefaultView.ToTable(true, "BookID", "BookName");
                for (int i = 0; i < dtBook.Rows.Count; i++)
                {
                    Custom_BookCatalog cusBC = new Custom_BookCatalog();
                    IList<Custom_Catalog> listC = new List<Custom_Catalog>();
                    DataRow drBook = dtBook.Rows[i];
                    cusBC.BookID = Convert.ToInt32(drBook["BookID"].ToString());
                    cusBC.BookName = drBook["BookName"].ToString();
                    //02.查询指定BookID下的一级目录列表
                    DataRow[] drCatalog = dt.Select("BookID=" + cusBC.BookID.ToString() + " and CatalogLevel=1", "Sort");
                    for (int j = 0; j < drCatalog.Length; j++)
                    {
                        Custom_Catalog cusC = new Custom_Catalog();
                        cusC.CatalogID = Convert.ToInt32(drCatalog[j]["CatalogID"].ToString());
                        cusC.CatalogName = drCatalog[j]["CatalogName"].ToString();
                        cusC.PageNo = Convert.ToInt32(drCatalog[j]["PageNo"].ToString());
                        //cusC.AnswerNum = Convert.ToInt32(drCatalog[j]["AnswerNum"].ToString());
                        cusC.AnswerNum = Convert.ToInt32(drCatalog[j]["AnswerNumShow"].ToString());
                        cusC.Sort = Convert.ToInt32(drCatalog[j]["Sort"].ToString());
                        cusC.Mp3Url = drCatalog[j]["Mp3Url"].ToString();
                        cusC.IsSubmit = Convert.ToInt32(drCatalog[j]["IsSubmit"].ToString());

                        //03.查询指定BookID和一级目录下的二级目录列表
                        DataRow[] drSecondCatalog = dt.Select("ParentID=" + drCatalog[j]["CatalogID"].ToString(), "Sort");
                        cusC.SecondCatalogList = new List<Custom_SecondCatalog>();
                        if (drSecondCatalog != null)
                        {
                            for (int k = 0; k < drSecondCatalog.Length; k++)
                            {
                                cusC.SecondCatalogList.Add(new Custom_SecondCatalog
                                {
                                    CatalogID = Convert.ToInt32(drSecondCatalog[k]["CatalogID"].ToString()),
                                    CatalogName = drSecondCatalog[k]["CatalogName"].ToString(),
                                    PageNo = Convert.ToInt32(drSecondCatalog[k]["PageNo"].ToString()),
                                    AnswerNum = Convert.ToInt32(drSecondCatalog[k]["AnswerNumShow"].ToString()),
                                    Sort = Convert.ToInt32(drSecondCatalog[k]["Sort"].ToString()),
                                    Mp3Url = drSecondCatalog[k]["Mp3Url"].ToString(),
                                    IsSubmit = Convert.ToInt32(drSecondCatalog[k]["IsSubmit"].ToString())
                                });
                            }
                        }
                        listC.Add(cusC);
                    }
                    cusBC.CatalogList = listC;
                    listBC.Add(cusBC);
                    if (listBC != null) return listBC;
                }

                return listBC;
            }
            return new List<Custom_BookCatalog>();
        }
        /// <summary>
        /// 导入目录信息，返回CatalogID（失败则为0）
        /// </summary>
        /// <param name="editionName"></param>
        /// <param name="bookName"></param>
        /// <param name="gradeid"></param>
        /// <param name="bookreel"></param>
        /// <param name="catalogName"></param>
        /// <param name="catalogLevel"></param>
        /// <param name="pageNo"></param>
        /// <param name="parentID"></param>
        /// <param name="sort"></param>
        ///   /// <param name="modEdtionId"></param>
        /// <returns></returns>
        public int ImportCatalogInfo(string editionName, string bookName, int gradeid, int bookreel, string catalogName, int catalogLevel, int pageNo, int parentID, int sort, string mp3Url, int modEdtionId, int bookId)
        {
            return catalogDAL.ImportCatalogInfo(editionName, bookName, gradeid, bookreel, catalogName, catalogLevel, pageNo, parentID, sort, mp3Url, modEdtionId, bookId);
        }
        /// <summary>
        /// 分页查询目录列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public IList<V_Catalog> GetCatalogPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            IList<V_Catalog> list = catalogDAL.GetCatalogPageList(pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
            return list == null ? (new List<V_Catalog>()) : list;
        }

        /// <summary>
        /// 获取下一目录的作业地址
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public V_Catalog GetNextCatalog(int catalogId)
        {
            var currentCatalog = catalogDAL.GetCatalog(catalogId);
            string bookId = currentCatalog.BookID.ToString();
            string strWhere = " 1=1 ";
            int totalCount = 0;
            int totalPages = 0;
            IList<V_Catalog> catalogs = GetCatalogPageList(1, int.MaxValue, strWhere + string.Format(" and CatalogID is not NULL and IsRemove=0  and BookID={0} and CatalogLevel=3 ", bookId), "EditionID,SubjectID,GradeID,BookReel,BookID,Sort", 1, out totalCount, out totalPages);
            var Catalogs = catalogs.ToList();
            var curIndex = Catalogs.IndexOf(catalogs.FirstOrDefault(o => o.CatalogID == catalogId));
            if (Catalogs[curIndex] != Catalogs.LastOrDefault())//不是最后一个
            {
                var nextCata = Catalogs[curIndex + 1];
                return nextCata;
            }
            else
            {
                return Catalogs[curIndex];//没有下一个了,返回当前目录
            }
        }
        public QTb_Catalog GetCatalog(int catalogid)
        {
            return catalogDAL.GetCatalog(catalogid);
        }

        public bool UpdateCatalog(QTb_Catalog catalog)
        {
            return catalogDAL.UpdateCatalog(catalog);
        }
        /// <summary>
        /// 移除指定目录的题目
        /// </summary>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public bool RemoveCatalogQuestions(int catalogid)
        {
            return catalogDAL.RemoveCatalogQuestions(catalogid);
        }

        public IList<QTb_Catalog> GetCatalogList(string strWhere, string orderby = "")
        {
            IList<QTb_Catalog> list = catalogDAL.GetCatalogList(strWhere, orderby);
            return list == null ? (new List<QTb_Catalog>()) : list;
        }

        public IList<V_Catalog> GetVCatalogList(string strWhere, string orderby = "")
        {
            IList<V_Catalog> list = catalogDAL.GetVCatalogList(strWhere, orderby);
            return list == null ? (new List<V_Catalog>()) : list;
        }
        /// <summary>
        /// 根据BookID得到目录信息
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        public DataSet GetCatalogByBookID(int BookID)
        {
            return catalogDAL.GetCatalogByBookID(BookID);
        }
        /// <summary>
        /// 根据ParentID获取学习记录信息
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public DataSet GetCatalogStudyByParentID(int ParentID)
        {
            return catalogDAL.GetCatalogStudyByParentID(ParentID);
        }
    }
}
