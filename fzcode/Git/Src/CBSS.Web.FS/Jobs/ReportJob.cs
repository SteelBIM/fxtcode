using CBSS.Core.Log;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract.IBSResource;
using CBSS.IBS.IBLL;
using CourseActivate.Web.FS.Controllers;
using FluentScheduler;
using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using CBSS.Tbx.BLL;
using CBSS.Tbx.IBLL;

namespace CourseActivate.Web.FS.Jobs
{
    public  class ReportJob : IJob
    {
        ITbxService tbxService =new TbxService();
        public void Execute()
        {
            lock (TimedTask._reportlock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4NetHelper.Info(LoggerType.ServiceExceptionLog,"初中英语学习报告入库开始");
                    tbxService.JuniorEnglishReport2DB();
                    Log4NetHelper.Info(LoggerType.ServiceExceptionLog, "初中英语学习报告入库结束");
                }
            }
        }
        
    }
}