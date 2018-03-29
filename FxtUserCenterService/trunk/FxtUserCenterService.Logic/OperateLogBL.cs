using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;

namespace FxtUserCenterService.Logic
{
    public class OperateLogBL
    {
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
            return OperateLogDA.InsertLog(SysTypeCode, CityId, FxtCompanyId, UserId, UserName, LogType, EventType, ObjectId, ObjectName, Remarks, WebIP);
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
        public static int SignIn(string UserId, int FxtCompanyId, string IPAddress, string PasCode, int SysTypeCode, int CityId, string BrowserType, string activeTime, string loginDate)
        {
            return OperateLogDA.SignIn(UserId, FxtCompanyId, IPAddress, PasCode, SysTypeCode, CityId, BrowserType, activeTime, loginDate);
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="pascode">唯一识别码</param>
        /// <param name="logOutDate">退出时间</param>
        /// <returns></returns>
        public static int SignOut(string pascode, string logOutDate, int cityId)
        {
            return OperateLogDA.SignOut(pascode, logOutDate, cityId);
        }
        /// <summary>
        /// 更新在线时间
        /// </summary>
        /// <param name="pascode">唯一识别码</param>
        /// <param name="activeTime">最后在线时间</param>
        /// <returns></returns>
        public static int UpdateActiveTime(string pascode, string activeTime)
        {
            return OperateLogDA.UpdateActiveTime(pascode, activeTime);
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
        public static void InsertApiInvokeLogUsercenter(int companyId, string functionname, int apitype, int producttypecode, string ip, string requestparameter)
        {
            OperateLogDA.InsertApiInvokeLogUsercenter(companyId, functionname, apitype, producttypecode, ip, requestparameter);
        }
    }
}
