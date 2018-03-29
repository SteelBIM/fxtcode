using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using FxtUserCenterService.Entity;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductAppDA : Base
    {
        public static int Add(CompanyProductApp model)
        {
            return InsertFromEntity<CompanyProductApp>(model);
        }
        public static int Update(CompanyProductApp model)
        {
            return UpdateFromEntity<CompanyProductApp>(model);
        }
        //批量更新
        public static int UpdateMul(CompanyProductApp model, int[] ids)
        {
            return UpdateFromIds<CompanyProductApp>(model, ids);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<CompanyProductApp>(id);
        }
        public static CompanyProductApp GetCompanyProductAppByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<CompanyProductApp>(id);
        }

        //这里要判断平台类型，是否能调用function kevin 20140330 
        public static CompanyProductApp GetAppkey(int appid, string apppwd, int systypecode, string functionName, string signname, string splatype)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql= SQL.ProductApp.GetAppkey;
            parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@appid", appid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@apppwd", apppwd, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@systypecode ", systypecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@functionname", functionName, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@splatype", splatype, SqlDbType.VarChar));
            return ExecuteToEntity<CompanyProductApp>(sql, CommandType.Text, parameters);    
        }

        /// <summary>
        /// 获得产品API信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="systypecode"></param>
        /// <param name="signname"></param>
        /// <returns></returns>
        public static List<CompanyProductApp> GetProductAPIInfo(int appid, int systypecode, string signname)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.ProductApp.GetProductAPIInfo;
            parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.VarChar));
            if (appid>0)
            {
                sql += " and cpa.AppId =@appid";
            }
            parameters.Add(SqlHelper.GetSqlParameter("@appid", appid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@systypecode ", systypecode, SqlDbType.Int));
            return ExecuteToEntityList<CompanyProductApp>(sql, CommandType.Text, parameters);
        }
    }
}
