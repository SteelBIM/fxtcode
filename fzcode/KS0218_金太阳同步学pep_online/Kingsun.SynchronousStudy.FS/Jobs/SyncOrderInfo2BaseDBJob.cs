using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class SyncOrderInfo2BaseDBJob:IJob
    {

        public void Execute()
        {
            lock (TimedTask._lock_Order2Base)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {

                    Log4Net.LogHelper.Info("同步学订单到总库同步开始");
                    new SyncAllDataBLL().SyncOrderInfo2BaseDB();
                    Log4Net.LogHelper.Info("同步学订单到总库同步结束");
                }
            }
        }
    }
}