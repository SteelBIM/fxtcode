using System;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class Log : ILog
    {
        public int InsertLog(int CityId, int FxtCompanyId, string UserId, string UserName, int LogType, int EventType, string ObjectId, string ObjectName, string Remarks, string WebIP)
        {
            try
            {
                string strsql = "INSERT INTO [FxtLog].[dbo].[SYS_Log]([SysType],[CityId],[FxtCompanyId],[UserId],[UserName],[LogType],[EventType],[ObjectId],[ObjectName],[Remarks],[WebIP])"
                    + " values (1003002," + CityId + "," + FxtCompanyId + ",'" + UserId + "','" + UserName + "'," + LogType + "," + EventType + ",'" + ObjectId + "','" + ObjectName + "','" + Remarks + "','" + WebIP + "')";
                if (UserName == "")
                {
                    strsql = "INSERT INTO [FxtLog].[dbo].[SYS_Log]([SysType],[CityId],[FxtCompanyId],[UserId],[UserName],[LogType],[EventType],[ObjectId],[ObjectName],[Remarks],[WebIP])"
                    + " select 1003002," + CityId + "," + FxtCompanyId + ",'" + UserId + "',(select UserName from FxtProject.dbo.Privi_user where Userid='" + UserId + "')," + LogType + "," + EventType + ",'" + ObjectId + "','" + ObjectName + "','" + Remarks + "','" + WebIP + "' ";
                }

                DBHelperSql.ConnectionString = ConfigurationHelper.FXTLog;
                return DBHelperSql.ExecuteNonQuery(strsql);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return 0;
            }
        }

        public int InsertOperateLog(int CityId, int FxtCompanyId, int typecode, string typecodeIDType, int typecodeIDValue, string fields, string Value1, string Value2, string UserName)
        {
            try
            {
                string strsql = "INSERT INTO [FxtLog].[dbo].[SYS_FxtProjectOperation_Log](SysType,Cityid,Fxtcompanyid,typecode,typecodeIDType,typecodeIDValue,fields,Value1,Value2,Creator)"
                    + " values (1003002," + CityId + "," + FxtCompanyId + "," + typecode + ",'" + typecodeIDType + "'," + typecodeIDValue + ",'" + fields + "','" + Value1 + "','" + Value2 + "','" + UserName + "')";

                DBHelperSql.ConnectionString = ConfigurationHelper.FXTLog;
                return DBHelperSql.ExecuteNonQuery(strsql);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
