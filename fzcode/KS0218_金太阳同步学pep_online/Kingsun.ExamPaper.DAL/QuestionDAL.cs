using System;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Kingsun.ExamPaper.DAL
{
    public class QuestionDAL : BaseManagement
    {
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="catalogid">目录id</param>
        /// <param name="isDo">0：查看，1：做题</param>
        /// <returns></returns>
        public DataSet GetQuestionList(string userid, int catalogid, int isDo)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            param[1] = new SqlParameter("@CatalogID", SqlDbType.Int);
            param[2] = new SqlParameter("@IsDo", SqlDbType.Int);

            param[0].Value = userid;
            param[1].Value = catalogid;
            param[2].Value = isDo;

            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetQuestionList", list);
        }

        public IList<V_Question> GetVQuestionList(string strWhere, string orderby = "")
        {
            return Search<V_Question>(strWhere, orderby);
        }

        public IList<Tb_QuestionInfo> GetQuestionPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            return GetPageList<Tb_QuestionInfo>("Tb_QuestionInfo", pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
        }

        public IList<V_Question> GetVQuestionPageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            return GetPageList<V_Question>("V_Question", pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
        }

        public Tb_QuestionInfo GetQuestion(string questionid)
        {
            return Select<Tb_QuestionInfo>(questionid);
        }
        /// <summary>
        /// 更新题目信息
        /// </summary>
        /// <param name="qInfo"></param>
        /// <returns></returns>
        public bool UpdateQuestion(Tb_QuestionInfo qInfo)
        {
            if (Update<Tb_QuestionInfo>(qInfo))
            {
                string strSql = "update Tb_QuestionInfo set QuestionTitle='" + qInfo.QuestionTitle + "',QuestionModel='" + qInfo.QuestionModel + "' where ";
                if (string.IsNullOrEmpty(qInfo.ParentID))
                {
                    //该题为大题时，同步更新小题
                    strSql += " ParentID='" + qInfo.QuestionID + "'";
                }
                else
                {
                    //该题为小题，同步更新大题和其他小题
                    strSql += " QuestionID='" + qInfo.ParentID + "' or ParentID='" + qInfo.ParentID + "'";
                }
                if (ExcuteSqlWithTran(strSql))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
