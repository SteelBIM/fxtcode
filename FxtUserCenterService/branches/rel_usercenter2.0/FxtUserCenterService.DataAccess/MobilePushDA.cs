using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity.DBEntity;

namespace FxtUserCenterService.DataAccess
{
    /// <summary>
    /// 手机推送 DA
    /// </summary>
    public class MobilePushDA : Base
    {

        public static int Add(DatMobilePush model)
        {
            return InsertFromEntity<DatMobilePush>(model);
        }

        public static int Update(DatMobilePush model)
        {
            return UpdateFromEntity<DatMobilePush>(model);
        }

      
        /// <summary>
        /// 获取用户 设备Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="producttypecode">产品</param>
        /// <returns></returns>
        public static List<DatMobilePush> GetCheckUser(string username, int? producttypecode)
        {
            string sql = @" select *from dbo.Dat_MobilePush with(nolock) where username=@username ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.NVarChar));

            if (producttypecode > 0)
            {
                sql += " and producttypecode=@producttypecode";
                parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", producttypecode, SqlDbType.Int));
            }

            return ExecuteToEntityList<DatMobilePush>(sql, CommandType.Text, parameters);
        }


        /// <summary>
        /// 更新手机设备
        /// </summary>
        /// <param name="IosPhshUserid"></param>
        /// <param name="AndPhshUserid"></param>
        /// <param name="channelid"></param>
        /// <param name="UserName"></param>
        /// <param name="ProductTypeCode"></param>
        /// <returns></returns>
        public static int UpdatePush(DatMobilePush model)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @" update dbo.Dat_MobilePush set IosPhshUserid=@IosPhshUserid,AndPhshUserid=@AndPhshUserid,channelid=@channelid where 
			                              UserName=@UserName and  ProductTypeCode=@ProductTypeCode ";
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@IosPhshUserid", string.IsNullOrEmpty(model.iosphshuserid)?"":model.iosphshuserid, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@AndPhshUserid", string.IsNullOrEmpty(model.andphshuserid)?"":model.andphshuserid, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@channelid", model.channelid, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@UserName", model.username, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@ProductTypeCode", model.producttypecode, SqlDbType.Int));
            return ExecuteNonQuery(cmd);
        }


        /// <summary>
        /// 退出清空手机设备Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int ExitReMoveDiviceId(DatMobilePush model)
        {
            string sql = @"update dbo.Dat_MobilePush set  AndPhshUserid='',IosPhshUserid='' where UserName=@username and ProductTypeCode=@producttypecode ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@username", model.username, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", model.producttypecode, SqlDbType.Int));
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 登录时保存token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="channelid"></param>
        /// <param name="productTypeCode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int Insert(string userName, string channelid, int productTypeCode, string token)
        {
            string strSql = string.Format(@"INSERT INTO [FxtUserCenter].[dbo].[Dat_MobilePush]
                                           ([UserName]
                                           ,[AndPhshUserid]
                                           ,[IosPhshUserid]
                                           ,[Channelid]
                                           ,[ProductTypeCode]
                                           ,[Token])
                                     VALUES ('{0}','','','{1}',{2},'{3}')",userName,channelid,productTypeCode,token);

            SqlCommand cmd = new SqlCommand(strSql);

            return ExecuteNonQuery(cmd);

        }
        /// <summary>
        /// 效验token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int VerifyToken(string token)
        {
            string strSql = string.Format(@"select count(*) from [Dat_MobilePush] where token = '{0}'",token);
         
            var result = ExecuteDataSet(strSql).Tables[0];

            return result.Rows.Count;
        }

    }
}
