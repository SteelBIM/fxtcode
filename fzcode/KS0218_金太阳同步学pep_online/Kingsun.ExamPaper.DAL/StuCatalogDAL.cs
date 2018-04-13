using Kingsun.ExamPaper.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Kingsun.ExamPaper.DAL
{
    public class StuCatalogDAL:BaseManagement
    {
        /// <summary>
        /// 获取学习报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public DataSet GetStuCatalog(string userid, int catalogid)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            param[1] = new SqlParameter("@CatalogID", SqlDbType.Int);

            param[0].Value = userid;
            param[1].Value = catalogid;

            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetStuCatalog", list);
        }

        /// <summary>
        /// 获取学习报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public DataSet GetDoDateStuCatalog(string userid, DateTime DoDate)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            param[1] = new SqlParameter("@DoDate", SqlDbType.DateTime);

            param[0].Value = userid;
            param[1].Value = DoDate;

            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetDoDateStuCatalog", list);
        }

        /// <summary>
        /// 获取班级学习报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public DataSet GetClassStuCatalogWeek(string UserIDs)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@UserIDs", SqlDbType.VarChar, 2000);
            param[0].Value = UserIDs;
            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetClassStuCatalogWeek", list);
        }

        /// <summary>
        /// 获取班级圈最佳报告
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public DataSet GetClassStuCatalog(string UserIDs, int? CatalogID, int isBest)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@UserIDs", SqlDbType.VarChar, 500);
            param[1] = new SqlParameter("@CatalogID", SqlDbType.Int);
            param[2] = new SqlParameter("@isBest", SqlDbType.Int);
            param[0].Value = UserIDs;
            param[1].Value = CatalogID;
            param[2].Value = isBest;
            List<DbParameter> list = new List<DbParameter>();
            list.AddRange(param);
            return ExecuteProcedure("Proc_GetClassStuCatalog", list);
        }
    }
}
