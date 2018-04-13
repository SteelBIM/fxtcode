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
    public class IBSRepairJob : IJob
    {
        static IMOD2IBSChangeBLL modchange = new MOD2IBSChangeBLL();

        public void Execute()
        {
            lock (TimedTask._repairlock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("IBS名称为空修复脚本");
                    modchange.IBSRepairTrueName();
                    modchange.IBSRepairClassUserTrueName();
                    Log4Net.LogHelper.Info("IBS名称为空修复脚本");
                }
            }
        }
    }
}