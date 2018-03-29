using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using CAS.Common;
using CAS.Entity;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize.Impl
{
    public partial class Implement
    {
        /// <summary>
        /// 写系统操作日志
        /// </summary>   
        /// <param name="SysTypeCode">系统类型</param>
        /// <param name="CityId">城市ID</param>
        /// <param name="FxtCompanyId">评估机构ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="UserName">用户名</param>
        /// <param name="LogType">对象类型7002006（楼盘、楼栋、房号、住宅案例）</param>
        /// <param name="EventType">操作类型7001001(新增、修改、删除)</param>
        /// <param name="ObjectId">对象ID</param>
        /// <param name="ObjectName">对象名称</param>
        /// <param name="Remarks">操作描述</param>
        /// <param name="WebIP">IP地址</param>
        /// <returns></returns>
        public WCFJsonData OperateLog(string sinfo, string info)
        {
            JObject objinfo = JObject.Parse(info);
            JObject appinfo = objinfo["appinfo"] as JObject;
            JObject uinfo = objinfo["uinfo"] as JObject;
            JObject funinfo = objinfo["funinfo"] as JObject;

            int systypecode = int.Parse(appinfo.Value<string>("systypecode"));
            int cityid = funinfo.Value<int>("cityid");
            int fxtcompanyid = funinfo.Value<int>("fxtcompanyid");
            string userid = funinfo.Value<string>("userid");
            string username = uinfo.Value<string>("username");
            int logtype = funinfo.Value<int>("logtype");
            int eventtype = funinfo.Value<int>("eventtype");
            string objectid = funinfo.Value<string>("objectid");
            string objectname = funinfo.Value<string>("objectname");
            string remarks = funinfo.Value<string>("remarks");
            string webip = funinfo.Value<string>("webip");

            int result = OperateLogBL.InsertLog(systypecode, cityid, fxtcompanyid, userid, username, logtype, eventtype, objectid, objectname, remarks, webip);

            if (result > 0)
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "插入操作日志失败");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="FxtCompanyId">评估机构ID</param>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="PasCdoe">唯一标识</param>
        /// <param name="SysTypeCode">系统类型</param>
        /// <param name="CityId">城市ID</param>
        /// <param name="BrowserType">浏览器（类型+版本号）</param>
        /// <returns></returns>
        public WCFJsonData SignIn(string sinfo, string info)
        {
            JObject objinfo = JObject.Parse(info);
            JObject appinfo = objinfo["appinfo"] as JObject;
            JObject uinfo = objinfo["uinfo"] as JObject;
            JObject funinfo = objinfo["funinfo"] as JObject;

            string userid = uinfo.Value<string>("username");
            int fxtcompanyid = funinfo.Value<int>("fxtcompanyid");
            string ipaddress = funinfo.Value<string>("ipaddress");
            string pascode = funinfo.Value<string>("pascode");
            int systypecode = int.Parse(appinfo.Value<string>("systypecode"));
            int cityid = funinfo.Value<int>("cityid");
            string browsertype = funinfo.Value<string>("browsertype");
            string activeTime = funinfo.Value<string>("activetime");
            string loginDate = funinfo.Value<string>("logindate");

            int result = OperateLogBL.SignIn(userid,fxtcompanyid,ipaddress,pascode,systypecode,cityid,browsertype, activeTime, loginDate);
            if (result > 0)
            {
                return JSONHelper.GetWcfJson(pascode, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "插入登录日志失败");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="pascode">登录唯一标识</param>
        /// <returns></returns>
        public WCFJsonData SignOut(string sinfo, string info)
        {
            JObject objinfo = JObject.Parse(info);
            JObject funinfo = objinfo["funinfo"] as JObject;

            string pascode = funinfo.Value<string>("pascode"); 
            string logOutDate = funinfo.Value<string>("logoutdate");
            int cityId = funinfo.Value<int>("cityId");
            int result = OperateLogBL.SignOut(pascode, logOutDate,cityId);
            if (result > 0)
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "插入退出日志失败");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="pascode">登录唯一标识</param>
        /// <returns></returns>
        public WCFJsonData UpdateActiveTime(string sinfo, string info)
        {
            JObject objinfo = JObject.Parse(info);
            JObject funinfo = objinfo["funinfo"] as JObject;

            string pascode = funinfo.Value<string>("pascode");
            string activeTime = funinfo.Value<string>("activetime");

            int result = OperateLogBL.UpdateActiveTime(pascode, activeTime);
            if (result > 0)
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Success, "成功");
            }
            else
            {
                return JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.Failure, "更新在线时间失败");
            }
        }
    }
}
