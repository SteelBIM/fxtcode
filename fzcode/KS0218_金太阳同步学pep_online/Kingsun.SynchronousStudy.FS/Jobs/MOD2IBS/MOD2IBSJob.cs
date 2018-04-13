using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.IBS.BLL.MOD2IBS;
using Kingsun.IBS.IBLL.IBS_MOD;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class MOD2IBSJob:IJob
    {
        static IMOD2IBSChangeBLL modchange = new MOD2IBSChangeBLL();
        void IJob.Execute()
        {
            lock (TimedTask._mod2ibs)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("MOD2IBS同步开始");
                    modchange.Change();
                    Log4Net.LogHelper.Info("MOD2IBS同步结束");
                }
            }
        }
    }
}