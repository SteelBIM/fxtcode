using System;
using System.Data.SqlClient;
using System.Data;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class SYS_LoginDAL : ISYS_Login
    {
        #region 登录日志
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="userId">用户名称</param>
        /// <param name="fxtCompanyId">公司Id</param>
        ///  <param name="cityId">城市ID</param>
        /// <param name="sysTypeCode">系统类型</param>
        /// <param name="pasCode">登录唯一标识</param>
        /// <returns></returns>
        public int AddSYS_Login(string userId, int fxtCompanyId, int cityId, int sysTypeCode, string pasCode, string ipAddress, string browserType)
        {
            string sql = @"INSERT INTO [FxtLog].[dbo].[SYS_Login] with(rowlock)
                           ([UserId],[FxtCompanyId],[LoginDate],[IPAddress],[PasCode],[SysTypeCode],[CityId],[BrowserType])
                     VALUES
                           (@UserId,@FxtCompanyId,@LoginDate,@IPAddress,@PasCode,@SysTypeCode,@CityId,@BrowserType)";
            try
            {
               
                var loginDate = DateTime.Now;

                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FXTLog))
                {
                    return conn.Execute(sql, new { UserId = userId, FxtCompanyId = fxtCompanyId, loginDate, ipAddress, CityId = cityId, SysTypeCode = sysTypeCode, PasCode = pasCode, browserType });
                }

            }
            catch(Exception ex)
            {
               throw new Exception(ex.Message);
            }

        }
        #endregion
        #region 退出日志
        /// <summary>
        /// 退出日志
        /// </summary>
        /// <param name="pasCode">登录唯一标识符</param>
        /// <param name="cityId">当前城市ID</param>
        /// <returns></returns>
        public int UpdateSYS_Login(string pasCode,int cityId)
        {

            try
            {
                string sql = @"UPDATE [FxtLog].[dbo].[SYS_Login] with(rowlock) SET [LogOutDate] =@LogOutDate,CityId=@CityId
                           WHERE PasCode=@PasCode ";
              
                using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FXTLog))
                {
                    return conn.Execute(sql, new { PasCode = pasCode, CityId = cityId, LogOutDate  = DateTime.Now});
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region 获取最后登录的记录
        /// <summary>
        /// 获取最后登录的记录
        /// </summary>
        /// <param name="UserId">用户名称admin@fxt.com</param>
        /// <param name="FxtCompanyId">公司Id 25</param>
        /// <param name="CityId">城市Id 6</param>
        /// <returns></returns>
        public SYS_Login GetSys_Login(string UserId, int FxtCompanyId)
        {
            try
            {
                string sql = @"select top 1 id, UserId, FxtCompanyId, LoginDate, LogOutDate, IPAddress,
                            PasCode, SysTypeCode,CityId, BrowserType 
                            from fxtlog.dbo.SYS_Login with(rowlock) 
                            where UserId=@UserId and FxtCompanyId=@FxtCompanyId
                            order by LoginDate desc";
                SqlParameter[] pa = {
                                    new SqlParameter("@UserId",UserId), 
                                    new SqlParameter("@FxtCompanyId",FxtCompanyId)};
                DBHelperSql.ConnectionString = ConfigurationHelper.FXTLog;
                return SqlModelHelper<SYS_Login>.GetSingleObjectBySql(sql, pa);
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
