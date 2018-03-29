using CAS.Common;
using CAS.Entity;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FxtCenterService.Logic
{
    public class ExecuteTimeLogBL
    {
        public static int Add(ExecuteTimeLog model)
        {
            //return ExecuteTimeLogDA.Add(model);
            return 1;
        }

        private static Thread BgTimeLogThread { set; get; }
        private static List<ExecuteTimeLog> loglist = new List<ExecuteTimeLog>();
        private static List<string> mysqlloglist = new List<string>();
        private static DateTime LastUpdateTime = DateTime.Now;
        private static Int32 lognum = Int32.Parse(WebCommon.GetConfigSetting("FxtDataCenterLogNum"));//获取日志批量处理量

        /// <summary>
        /// sql写入日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddList(ExecuteTimeLog log)
        {
            try
            {
                lock (loglist)//锁住日志列表
                {
                    loglist.Add(log);
                    if (loglist.Count >= lognum || LastUpdateTime.AddMinutes(1) < DateTime.Now)
                    {
                        ThreadStart ts = new ThreadStart(ExecuteTimeLogList);
                        BgTimeLogThread = new Thread(ts);
                        BgTimeLogThread.IsBackground = true;
                        BgTimeLogThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "AddList增加日志出错");
            }
        }

        /// <summary>
        /// mysql增加日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddListMySql(ExecuteTimeLog mylog)
        {
            try
            {
                List<ExecuteTimeLog> addlist = new List<ExecuteTimeLog>();
                lock (loglist)//锁住日志列表
                {
                    loglist.Add(mylog);
                    if (loglist.Count >= lognum || LastUpdateTime.AddMinutes(1) < DateTime.Now)
                    {
                        foreach (ExecuteTimeLog log in loglist)
                        {
                            addlist.Add(log);
                        }
                        loglist.Clear();
                        LastUpdateTime = DateTime.Now;
                    }
                }

                if (addlist != null && addlist.Count > 0)
                {
                    IEnumerable<dynamic> loglistgroup = (
                        from c in addlist
                        where c.totaltime <= 2000  //2000以内，算均值
                        group c by new { c.functionname, c.requestparam, c.fxtcompanyid, c.systypecode }
                            into g
                            select new
                            {
                                functionname = g.Key.functionname,
                                usercentertime = g.Average(c => c.usercentertime),
                                cityauthoritytime = g.Average(c => c.cityauthoritytime),
                                overflowtime = g.Average(c => c.overflowtime),
                                getdatatime = g.Average(c => c.getdatatime),
                                totaltime = g.Average(c => c.totaltime),
                                sqltime = g.Average(c => c.sqltime),
                                ident = new Guid(),
                                time = g.Count().ToString(),  //存放请求次数
                                addtime = DateTime.Now,
                                requestparam = g.Key.requestparam,  //存放请求来源
                                serverid = "",
                                code = "",
                                starttime = g.Max(c => c.starttime),
                                fxtcompanyid = g.Key.fxtcompanyid,
                                systypecode = g.Key.systypecode,
                                sqlconnetiontime = 0,
                                sqlexecutetime = g.Average(c => c.sqlexecutetime),
                                groupnum = g.Count()
                            });
                    loglistgroup = loglistgroup.Concat(addlist.Where(o => o.totaltime > 2000).Select(o => new
                    {
                        functionname = o.functionname,
                        usercentertime = o.usercentertime,
                        cityauthoritytime = o.cityauthoritytime,
                        overflowtime = o.overflowtime,
                        getdatatime = o.getdatatime,
                        totaltime = o.totaltime,
                        sqltime = o.sqltime,
                        ident = new Guid(),
                        time = "1",  //存放请求次数
                        addtime = DateTime.Now,
                        requestparam = o.requestparam + "单次",  //存放请求来源
                        serverid = "",
                        code = "",
                        starttime = o.starttime,
                        fxtcompanyid = o.fxtcompanyid,
                        systypecode = o.systypecode,
                        sqlconnetiontime = 0,
                        sqlexecutetime = o.sqlexecutetime,
                        groupnum = 0
                    }));

                    string sql = "";
                    foreach (var log in loglistgroup)
                    {
                        ExecuteTimeLog lg = new ExecuteTimeLog();
                        if (log.functionname == null)
                        {
                            lg.functionname = "";
                        }
                        else
                        {
                            lg.functionname = log.functionname;
                        }
                        if (log.usercentertime == null)
                        {
                            lg.usercentertime = 0;
                        }
                        else
                        {
                            if (log.usercentertime < 0)
                            {
                                lg.usercentertime = 0;
                            }
                            else
                            {
                                lg.usercentertime = (Int32)log.usercentertime;
                            }

                        }
                        if (log.cityauthoritytime == null) { lg.cityauthoritytime = 0; }
                        else
                        {
                            if (log.cityauthoritytime < 0)
                            {
                                lg.cityauthoritytime = 0;
                            }
                            else
                            {
                                lg.cityauthoritytime = (Int32)log.cityauthoritytime;
                            }
                        }
                        if (log.overflowtime == null) { lg.overflowtime = 0; }
                        else
                        {
                            if (log.cityauthoritytime < 0)
                            {
                                lg.overflowtime = 0;
                            }
                            else
                            {
                                lg.overflowtime = (Int32)log.overflowtime;
                            }
                        }
                        if (log.getdatatime == null) { lg.getdatatime = 0; }
                        else
                        {
                            if (log.getdatatime < 0)
                            {
                                lg.getdatatime = 0;
                            }
                            else
                            {
                                lg.getdatatime = (Int32)log.getdatatime;
                            }
                        }
                        if (log.totaltime == null) { lg.totaltime = 0; }
                        else
                        {
                            if (log.totaltime < 0)
                            {
                                lg.totaltime = 0;
                            }
                            else
                            {
                                lg.totaltime = (Int32)log.totaltime;
                            }
                        }
                        if (log.sqltime == null) { lg.sqltime = 0; }
                        else
                        {
                            if (log.sqltime < 0)
                            {
                                lg.sqltime = 0;
                            }
                            else
                            {
                                lg.sqltime = (Int32)log.sqltime;
                            }
                        }

                        lg.ident = new Guid();

                        if (log.time == null) { lg.time = "0"; }
                        else
                        {
                            lg.time = log.time;
                        }
                        if (log.addtime == null) { lg.addtime = DateTime.Now; }
                        else
                        {
                            lg.addtime = log.addtime;
                        }
                        if (log.requestparam == null) { lg.requestparam = ""; }
                        else
                        {
                            lg.requestparam = log.requestparam;
                        }
                        if (log.serverid == null) { lg.serverid = ""; }
                        else
                        {
                            lg.serverid = log.serverid;
                        }
                        if (log.code == null) { lg.code = ""; }
                        else
                        {
                            lg.code = log.code;
                        }
                        if (log.starttime == null) { lg.starttime = DateTime.Now; }
                        else
                        {
                            lg.starttime = log.starttime;
                        }
                        if (log.fxtcompanyid == null) { lg.fxtcompanyid = 0; }
                        else
                        {
                            lg.fxtcompanyid = log.fxtcompanyid;
                        }
                        if (log.systypecode == null) { lg.systypecode = 0; }
                        else
                        {
                            lg.systypecode = log.systypecode;
                        }
                        if (log.sqlconnetiontime == null) { lg.sqlconnetiontime = 0; }
                        else
                        {
                            lg.sqlconnetiontime = log.sqlconnetiontime;
                        }
                        if (log.sqlexecutetime == null) { lg.sqlexecutetime = 0; }
                        else
                        {
                            if (log.sqlexecutetime < 0)
                            {
                                lg.sqlexecutetime = 0;
                            }
                            else
                            {
                                lg.sqlexecutetime = (Int32)log.sqlexecutetime;
                            }
                        }

                        sql = sql +
                          ",(" +
                          "'" + lg.functionname + "'," +
                          lg.usercentertime + "," +
                          lg.cityauthoritytime + "," +
                          lg.overflowtime + "," +
                          lg.getdatatime + "," +
                          lg.totaltime + "," +
                          lg.sqltime + "," +
                          "''," +   //ident
                          "'" + lg.time + "'," +
                          "'" + lg.addtime + "'," +
                          "'" + lg.requestparam + "'," +
                          "'" + lg.serverid + "'," +
                          "'" + lg.code + "'," +
                          "'" + lg.starttime + "'," +
                          lg.fxtcompanyid + "," +
                          lg.systypecode + "," +
                          lg.sqlconnetiontime + "," +
                          lg.sqlexecutetime + "," +
                          "'')";
                    }

                    sql = @"insert into fdc_execute_time_log(
                                  FunctionName,UserCenterTime,CityAuthorityTime,OverFlowTime,
                                  GetDataTime,TotalTime,SqlTime,Ident,TIME,ADDTIME,RequestParam,
                                  ServerId,CODE,StartTime,FxtCompanyId,SysTypeCode,SqlConnetionTime,
                                  SqlExecuteTime,Remark ) values"
                            + sql.Substring(1);

                    addlist.Clear();
                    //ExecuteTimeLogDA.AddListMyssql(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "AddList增加日志出错");
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        private static void ExecuteTimeLogList()
        {
            try
            {
                //if (loglist.Count >= lognum || LastUpdateTime.AddMinutes(1) < DateTime.Now)
                //{
                //    DataSet ds = null;
                //    lock (loglist)
                //    {
                //        if (loglist.Count >= 100 || LastUpdateTime.AddMinutes(1) < DateTime.Now)
                //        {
                //            ds = ExecuteTimeLogDA.GeExecuteTimeLogData(loglist);
                //            loglist.Clear();
                //            LastUpdateTime = DateTime.Now;
                //        }
                //    }
                //    if (ds != null)
                //    {
                //        ExecuteTimeLogDA.AddList(ds);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "ExecuteTimeLogList批量处理日志出错");
            }
        }

        /// <summary>
        /// 系统异常，日志处理
        /// </summary>
        private static void PushAllExecuteTimeLogList()
        {
            try
            {
                //DataSet ds = null;
                //lock (loglist)
                //{
                //    ds = ExecuteTimeLogDA.GeExecuteTimeLogData(loglist);
                //    loglist.Clear();
                //    LastUpdateTime = DateTime.Now;
                //}
                //if (ds != null)
                //{
                //    ExecuteTimeLogDA.AddList(ds);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "PushAllExecuteTimeLogList批量处理日志出错");
            }
        }

        public static int Add09(ExecuteTimeLog model)
        {
            //return ExecuteTimeLogDA.Add09(model);
            return 1;
        }
    }
}
