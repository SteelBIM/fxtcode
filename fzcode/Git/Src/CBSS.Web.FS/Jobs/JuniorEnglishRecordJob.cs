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
    /// <summary>
    /// 初中英语记录
    /// </summary>
    public class JuniorEnglishRecordJob
    {
        IIBSService ibsService = new IBSService();
        ITbxService tbxService = new TbxService();



        public void JuniorEnglishRecordExecute()
        {
            lock (TimedTask._JuniorEnglishRecordlock)
            {
                try
                {
                    tbxService.JuniorEnglishReport2DB();
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "初中英语学习记录入库异常", ex);
                }
            }
        }

        public void JuniorEnglishSpokenRecordExecute()
        {
            lock (TimedTask._JuniorEnglishSpokenRecordock)
            {
                try
                {
                    tbxService.JuniorEnglishSpokenRecord2DB();
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "初中英语口讯记录入库异常", ex);
                }
            }
        }

    }
}