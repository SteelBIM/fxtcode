using CAS.Common;
using CAS.Entity;
using FxtCenterService.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        public static void AddExecuteTimeLog(JObject funinfo, UserCheck company)
        {
            ExecuteTimeLog log = new ExecuteTimeLog();
            log.functionname = funinfo.Value<string>("functionname");
            log.usercentertime = StringHelper.TryGetInt(funinfo.Value<string>("usercentertime"));
            log.cityauthoritytime = StringHelper.TryGetInt(funinfo.Value<string>("cityauthoritytime"));
            log.overflowtime = StringHelper.TryGetInt(funinfo.Value<string>("overflowtime"));
            log.getdatatime = StringHelper.TryGetInt(funinfo.Value<string>("getdatatime"));
            log.totaltime = StringHelper.TryGetInt(funinfo.Value<string>("totaltime"));
            log.sqltime = StringHelper.TryGetInt(funinfo.Value<string>("sqltime"));
            //log.ident = Guid.Parse(funinfo.Value<string>("ident"));
            log.time = funinfo.Value<string>("time");
            log.addtime = StringHelper.TryGetDateTime(funinfo.Value<string>("addtime"));
            log.requestparam = funinfo.Value<string>("requestparam");
            log.serverid = funinfo.Value<string>("serverid");
            log.code = funinfo.Value<string>("code");
            log.starttime = StringHelper.TryGetDateTime(funinfo.Value<string>("starttime"));
            log.fxtcompanyid = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));
            log.systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            log.sqlconnetiontime = StringHelper.TryGetInt(funinfo.Value<string>("sqlconnetiontime"));
            log.sqlexecutetime = StringHelper.TryGetInt(funinfo.Value<string>("sqlexecutetime"));


            ExecuteTimeLogBL.Add(log);
        }

        public static void AddExecuteTimeCountLog(JObject funinfo, UserCheck company)
        {
            ExecuteTimeCountLog log = new ExecuteTimeCountLog();
            log.serverid = funinfo.Value<string>("serverid");
            log.executetime = StringHelper.TryGetDateTime(funinfo.Value<string>("executetime"));
            log.total = StringHelper.TryGetInt(funinfo.Value<string>("total"));

            ExecuteTimeCountLogBL.Add(log);
        }
    }
}
