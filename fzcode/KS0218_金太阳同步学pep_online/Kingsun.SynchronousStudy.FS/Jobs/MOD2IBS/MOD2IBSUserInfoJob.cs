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
    public class MOD2IBSUserInfoJob : IJob
    {
        static IMOD2IBSChangeBLL modchange = new MOD2IBSChangeBLL();
        void IJob.Execute()
        {
            lock (TimedTask._lock_UserInfo)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {

                    Log4Net.LogHelper.Info("MOD初始化用户数据开始");
                    modchange.MOD2IBSUserInfo();
                    Log4Net.LogHelper.Info("MOD初始化用户数据结束");
                }
            }
        }
    }
}