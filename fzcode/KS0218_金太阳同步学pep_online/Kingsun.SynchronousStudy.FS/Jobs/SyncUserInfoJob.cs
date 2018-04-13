using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class SyncUserInfoJob:IJob
    {
        public void Execute()
        {
            lock (TimedTask._lock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("同步学用户同步开始");
                    new SyncAllDataBLL().SyncUserInfo();
                    Log4Net.LogHelper.Info("同步学用户同步结束");
                }
            }
        }
    }
}