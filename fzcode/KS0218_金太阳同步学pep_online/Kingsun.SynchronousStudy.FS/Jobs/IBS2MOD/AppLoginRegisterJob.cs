using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.IBS.BLL.FZUUMS_UserService;
using Kingsun.IBS.BLL.IBSData;
using Kingsun.IBS.IBLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs.IBS2MOD
{
    public class AppLoginRegisterJob:IJob
    {
        private IAppLoginRegisterBLL app = new AppLoginRegisterBLL();

        public void Execute()
        {
            lock (TimedTask._ibs2modlock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("IBS新注册用户到MOD开始");
                    app.IBSRegisterUser2Mod();
                    Log4Net.LogHelper.Info("IBS新注册用户到MOD结束");
                }
            }
        }
    }     
    
}