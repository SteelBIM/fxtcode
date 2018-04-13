using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Kingsun.ExamPaper.DAL
{
    public class CatalogDAL : BaseManagement
    {
        /// <summary>
        /// 根据ParentID获取学习记录信息
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public DataSet GetCatalogStudyByParentID(int ParentID)
        {
            string sql = string.Format(@"select a.CatalogID,a.CatalogName,COUNT(StuCatID)Num from [QTb_Catalog] a
 left join [Tb_StuCatalog] b on a.CatalogID=b.CatalogID where ParentID='{0}'
 group by a.CatalogID,a.CatalogName", ParentID);
            return SqlHelper.ExecuteDataset(AppSetting.ConnectionString, CommandType.Text, sql);
        }
        /// <summary>
        /// 根据BookID得到目录信息
        /// </summary>
        /// <param name="BookID"></param>
        /// <returns></returns>
        public DataSet GetCatalogByBookID(int BookID)
        {
            //            string sql = string.Format(@"SELECT a.CatalogID,a.CatalogName,b.CatalogName ParentCatalogName,a.ParentID  
            //FROM [FZ_ExamPaper].[dbo].[QTb_Catalog] a
            //left join [FZ_ExamPaper].[dbo].[QTb_Catalog] b
            //on a.BookID=b.BookID and a.ParentID=b.CatalogID
            //where a.BookID='{0}' and a.CatalogLevel=2", BookID);
            string sql = string.Format(@"SELECT a.CatalogID,a.CatalogName,c.CatalogName ParentCatalogName,a.ParentID,ISNULL(num,0)Num
            FROM [FZ_ExamPaper].[dbo].[QTb_Catalog] a
            left join 
            (
	            select b.ParentID,COUNT(*) num from [Tb_StuCatalog] a
	            left join [QTb_Catalog] b
	            on a.CatalogID=b.CatalogID
	            where b.BookID='{0}'  
	            group by b.ParentID
            )b
            on a.CatalogID=b.ParentID
            left join [QTb_Catalog] c on a.BookID=c.BookID and a.ParentID=c.CatalogID
            where a.BookID='{0}' and a.CatalogLevel=2", BookID);
            return SqlHelper.ExecuteDataset(AppSetting.ConnectionString, CommandType.Text, sql);
        }
        /// <summary>
        /// 获取课本及目录信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="mod_ed">mod对应版本ID</param>
        /// <param name="gradeid">年级ID</param>
        /// <param name="bookreel">册别</param>
        /// <param name="subjectid">学科（默认为英语：3）</param>
        /// <returns></returns>
        public DataSet GetBookAndCatalogList(int? BookID, string UserID)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@BookID", SqlDbType.Int);
            param[1] = new SqlParameter("@UserID", SqlDbType.NVarChar);

            param[0].Value = BookID;
            param[1].Value = UserID;

            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetBookAndCatalogList", list);
        }

        public QTb_Catalog GetCatalog(int catalogid)
        {
            return Select<QTb_Catalog>(catalogid);
        }

        public bool UpdateCatalog(QTb_Catalog catalog)
        {
            return Update<QTb_Catalog>(catalog);
        }

        public IList<QTb_Catalog> GetCatalogList(string strWhere, string orderby = "")
        {
            return Search<QTb_Catalog>(strWhere, orderby);
        }

        public IList<V_Catalog> GetVCatalogList(string strWhere, string orderby = "")
        {
            return Search<V_Catalog>(strWhere, orderby);
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
        /// <param name="mp3Url"></param>
        ///        /// <param name="modEdtionId"></param>
        /// <returns></returns>
        public int ImportCatalogInfo(string editionName, string bookName, int gradeid, int bookreel, string catalogName, int catalogLevel, int pageNo, int parentID, int sort, string mp3Url, int modEdtionId, int bookId)
        {
            SqlParameter[] param = new SqlParameter[12];
            param[0] = new SqlParameter("@EditionName", SqlDbType.VarChar, 150);
            param[1] = new SqlParameter("@BookName", SqlDbType.NVarChar, 50);
            param[2] = new SqlParameter("@GradeID", SqlDbType.Int);
            param[3] = new SqlParameter("@BookReel", SqlDbType.Int);
            param[4] = new SqlParameter("@CatalogName", SqlDbType.VarChar, 50);
            param[5] = new SqlParameter("@CatalogLevel", SqlDbType.Int);
            param[6] = new SqlParameter("@PageNo", SqlDbType.Int);
            param[7] = new SqlParameter("@ParentID", SqlDbType.Int);
            param[8] = new SqlParameter("@Sort", SqlDbType.Int);
            param[9] = new SqlParameter("@Mp3Url", SqlDbType.VarChar, 254);
            param[10] = new SqlParameter("@MOD_ED", SqlDbType.Int);
            param[11] = new SqlParameter("@BookIDFromMod", SqlDbType.Int);

            param[0].Value = editionName;
            param[1].Value = bookName;
            param[2].Value = gradeid;
            param[3].Value = bookreel;
            param[4].Value = catalogName;
            param[5].Value = catalogLevel;
            param[6].Value = pageNo;
            param[7].Value = parentID;
            param[8].Value = sort;
            param[9].Value = mp3Url;
            param[10].Value = modEdtionId;
            param[11].Value = bookId;

            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            DataSet ds = ExecuteProcedure("Proc_ImportCatalogInfo", list);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public IList<V_Catalog> GetCatalogPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            return GetPageList<V_Catalog>("V_Catalog", pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
        }
        /// <summary>
        /// 移除指定目录下的题目
        /// </summary>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public bool RemoveCatalogQuestions(int catalogid)
        {
            string strSql = string.Format("delete from Tb_StuAnswer where CatalogID={0};delete from Tb_StuCatalog where CatalogID={0};"
                        + "delete from Tb_BlankAnswer where QuestionID in (select QuestionID from Tb_QuestionInfo where CatalogID={0});"
                        + "delete from Tb_SelectItem where QuestionID in (select QuestionID from Tb_QuestionInfo where CatalogID={0});"
                        + "delete from Tb_QuestionInfo where CatalogID={0};", catalogid);
            return ExcuteSqlWithTran(strSql);
        }


    }
}
