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
    public class ExecuteTimeLogDA
    {
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
                    loglist.Add(log);//存入LIST
                    if (loglist.Count >= lognum || LastUpdateTime.AddMinutes(1) < DateTime.Now)
                    {
                        ThreadStart ts = new ThreadStart(ExecuteTimeLogList);//异步
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
                //            ds = ExecuteTimeLogDA.GeExecuteTimeLogData(loglist); //List存入新List
                //            loglist.Clear();
                //            LastUpdateTime = DateTime.Now;
                //        }
                //    }
                //    if (ds != null)
                //    {
                //        ExecuteTimeLogDA.AddList(ds); //批量写入
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "ExecuteTimeLogList批量处理日志出错");
            }
        }

    }
}
