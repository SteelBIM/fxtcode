using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface ISYS_Login
    {
         /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="userId">用户名称</param>
        /// <param name="fxtCompanyId">公司Id</param>
        ///  <param name="cityId">城市ID</param>
        /// <param name="sysTypeCode">系统类型</param>
        ///  <param name="pasCode">登录唯一标识</param>
        /// <returns></returns>
        int AddSYS_Login(string userId, int fxtCompanyId, int cityId, int sysTypeCode, string pasCode, string ipAddress, string browserType);

         /// <summary>
        /// 退出日志
        /// </summary>
        /// <param name="pasCode">登录唯一标识符</param>
        /// <param name="cityId">当前城市ID</param>
        /// <returns></returns>
        int UpdateSYS_Login(string pasCode,int cityId);

         /// <summary>
        /// 获取最后登录的记录
        /// </summary>
        /// <param name="UserId">用户名称admin@fxt.com</param>
        /// <param name="FxtCompanyId">公司Id 25</param>
        /// <returns></returns>
        SYS_Login GetSys_Login(string UserId, int FxtCompanyId);
    }
}
