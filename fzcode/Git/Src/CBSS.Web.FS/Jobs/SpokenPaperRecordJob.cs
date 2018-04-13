using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBSS.Core.Log;
using CBSS.Tbx.BLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.FS.Controllers;
using FluentScheduler;

namespace CourseActivate.Web.FS.Jobs
{
    public class SpokenPaperRecordJob : IJob
    {
        ITbxService tbxService = new TbxService();
        public void Execute()
        {
            lock (TimedTask._spokenJoblock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4NetHelper.Info(LoggerType.ServiceExceptionLog, "初中英语口讯记录入库开始");
                    tbxService.JuniorEnglishSpokenRecord2DB();
                    Log4NetHelper.Info(LoggerType.ServiceExceptionLog, "初中英语口讯记录入库结束");
                }
            }
        }

    }
}