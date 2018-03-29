using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using CAS.Common;

namespace FxtUserCenterService.DataAccess
{
   public class OperateLogDA : BaseDA
    {
        private static readonly string openplatformContextLogStr = ConfigurationManager.ConnectionStrings["OpenplatformContext"].ToString();//默认数据库

       /// <summary>
       /// 操作日志
       /// </summary>
       /// <param name="SysTypeCode">系统code</param>
       /// <param name="CityId">城市ID</param>
       /// <param name="FxtCompanyId">评估机构ID</param>
       /// <param name="UserId">用户ID</param>
       /// <param name="UserName">账号</param>
       /// <param name="LogType">日志类型</param>
       /// <param name="EventType">事件类型</param>
       /// <param name="ObjectId">操作对象ID</param>
       /// <param name="ObjectName">操作对象名称</param>
       /// <param name="Remarks">备注</param>
       /// <param name="WebIP">IP</param>
       /// <returns></returns>
       public static int InsertLog(int SysTypeCode, int CityId, int FxtCompanyId, string UserId, string UserName, int LogType, int EventType, string ObjectId, string ObjectName, string Remarks, string WebIP)
       {

           string strSql = SQLName.FxtLog.InsertLog;
        
           SqlCommand cmd = new SqlCommand();
           cmd.CommandText = strSql;
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@SysType", SysTypeCode, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", CityId, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@FxtCompanyId", FxtCompanyId, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@UserId", UserId, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@UserName", UserName, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@LogType", LogType, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@EventType", EventType, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@ObjectId", ObjectId, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@ObjectName", ObjectName, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@Remarks", Remarks, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@WebIP", WebIP, SqlDbType.NVarChar));
           return ExecuteNonQuery(cmd);  
       }
       /// <summary>
       /// 登录
       /// </summary>
       /// <param name="UserId">用户ID</param>
       /// <param name="FxtCompanyId">评估机构ID</param>
       /// <param name="IPAddress">IP</param>
       /// <param name="PasCdoe">唯一识别码</param>
       /// <param name="SysTypeCode">系统Code</param>
       /// <param name="CityId">城市ID</param>
       /// <param name="BrowserType">浏览器类型</param>
       /// <returns></returns>
       public static int SignIn(string UserId, int FxtCompanyId,string IPAddress,string PasCdoe,int SysTypeCode, int CityId,string BrowserType,string activeTime,string loginDate)
       {
           string strSql = SQLName.FxtLog.SignIn;

           SqlCommand cmd = new SqlCommand();
           cmd.CommandText = strSql;
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@UserId", UserId, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@FxtCompanyId", FxtCompanyId, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@LoginDate", loginDate, SqlDbType.DateTime));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@activeTime", activeTime, SqlDbType.DateTime));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@IPAddress", IPAddress, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@PasCode", PasCdoe, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@SysTypeCode", SysTypeCode, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", CityId, SqlDbType.Int));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@BrowserType", BrowserType, SqlDbType.NVarChar));
           return ExecuteNonQuery(cmd);  
       }

       /// <summary>
       /// 退出
       /// </summary>
       /// <param name="pascode">唯一识别码</param>
       /// <param name="logOutDate">退出时间</param>
       /// <returns></returns>
       public static int SignOut(string pascode, string logOutDate,int cityId)
       { 
           string strSql = SQLName.FxtLog.SignOut;

           SqlCommand cmd = new SqlCommand();
           cmd.CommandText = strSql;
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@LogOutDate", logOutDate, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@PasCode", pascode, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.NVarChar));
           return ExecuteNonQuery(cmd);  
       }
       /// <summary>
       /// 更新在线时间
       /// </summary>
       /// <param name="pascode">唯一识别码</param>
       /// <param name="activeTime">最后在线时间</param>
       /// <returns></returns>
       public static int UpdateActiveTime(string pascode, string activeTime)
       {
           string strSql = SQLName.FxtLog.UpdateActiveTime;

           SqlCommand cmd = new SqlCommand();
           cmd.CommandText = strSql;
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@ActiveTime", activeTime, SqlDbType.NVarChar));
           cmd.Parameters.Add(SqlHelper.GetSqlParameter("@PasCode", pascode, SqlDbType.NVarChar));
           return ExecuteNonQuery(cmd);
       }

       /// <summary>
       /// 用户中心访问日志记录
       /// zhoub 20161010
       /// </summary>
       /// <param name="companyId">请求公司ID</param>
       /// <param name="functionname">接口名</param>
       /// <param name="apitype">操作类型</param>
       /// <param name="producttypecode">产口code</param>
       /// <param name="ip">IP地址</param>
       /// <param name="requestparameter">请求参数</param>
       public static void InsertApiInvokeLogUsercenter(int companyId,string functionname,int apitype,int producttypecode,string ip,string requestparameter)
       {
           try
           {
               string strSql = "INSERT INTO openplatform.api_invoke_log_usercenter(CompanyId,InvokeTime,FunctionName,APIType,ProductTypeCode,ip,RequestParameter) VALUES(" + companyId + ",NOW(),'" + functionname + "'," + apitype + "," + producttypecode + ",'" + ip + "','" + requestparameter + "')";
               using (var conn = new MySqlConnection(openplatformContextLogStr))
               {
                   conn.Open();
                   MySqlCommand command = conn.CreateCommand();
                   command.CommandText = strSql;
                   command.CommandTimeout = 0;
                   command.ExecuteNonQuery();
               }
           }
           catch (Exception ex)
           {
               LogHelper.Error(ex, "用户中心接口访问日志出错InsertApiInvokeLogUsercenter");
               throw;
           }
       }
    }
}
