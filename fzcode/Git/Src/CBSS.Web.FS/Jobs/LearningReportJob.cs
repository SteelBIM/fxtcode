using CBSS.Core.Log;
using CBSS.Tbx.BLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.FS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.FS.Jobs
{
    public class LearningReportJob
    {
        ITbxService tbxService = new TbxService();
        public void ExcutedLearningReport()
        {
            lock (TimedTask._LearningReportLock)
            {
                try
                {
                    tbxService.ExecuteLearningReport();
                    // ibsService.GetDubbingByCataId(13989);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "同步订单到优学异常", ex);
                }

            }
        }


        public void ExecuteStudentClassChangeLearningReport()
        {
            lock (TimedTask._classChangeLock)
            {
                try
                {
                    tbxService.ExecuteStudentClassChangeLearningReport();
                    // ibsService.GetDubbingByCataId(13989);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "同步订单到优学异常", ex);
                }

            }
        }

    }
}