using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TestLog4Net;
using CourseActivate.Activate.BLL;
namespace CourseActivate.Web.FS.Controllers
{

    /// <summary>
    /// TimedTask 的摘要说明
    /// </summary>
    public class TimedTask : IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        ActivateCourseBLL _activateCourseBll = new ActivateCourseBLL();

        public Registry Start()
        {
            HostingEnvironment.RegisterObject(this);
            Registry registry = new Registry();
            //registry.Schedule(() => Execute()).ToRunEvery(1).Days().At(14, 45);//每天几点执行一次代码
            registry.Schedule(() => Execute()).ToRunEvery(Core.Utility.ConfigItemHelper.SyncDBTimes).Seconds();
            registry.Schedule(() => ExecuteGeneric(_activateCourseBll.ActivateMonthRecordStatistics, ("激活码使用统计" + DateTime.Now.AddDays(-1).ToShortDateString()))).ToRunEvery(1).Days().At(1, 0);
            return registry;
        }

        /// <summary>
        /// 定时任务统一方法.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="task"></param>
        public void ExecuteGeneric(Func<bool> func, string task)
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                else
                {
                    bool success = false;
                    try
                    {
                        success = func();
                    }
                    catch (Exception ex)
                    {
                        TestLog4Net.LogHelper.Info("执行任务" + task + "出错:" + ex.Message);
                    }
                }
            }
        }
        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                else
                {
                    bool success = false;
                    try
                    {
                        success = new Activate.BLL.ActivateCourseBLL().SyncRedisToDB();
                    }
                    catch (Exception ex)
                    {
                        TestLog4Net.LogHelper.Info("执行异步保存redis数据到DB任务出错:" + ex.Message);
                    }
                    //这里写每天固定时间要运行的代码
                }
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

    }
}
