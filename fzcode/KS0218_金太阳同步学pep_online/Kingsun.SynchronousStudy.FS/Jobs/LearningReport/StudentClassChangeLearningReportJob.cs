using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.IBS.BLL.IBSLearningReport;
using Kingsun.IBS.IBLL.IBSLearningReport;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class StudentClassChangeLearningReportJob:IJob
    {
        private static IIBSLearningReport report = new IBSLearningReportBLL();
        void IJob.Execute()
        {
            lock (TimedTask._classChangeLearningReport)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("班级变更同步学习报告记录到Redis开始");
                    report.ExecuteStudentClassChangeLearningReport();
                    Log4Net.LogHelper.Info("班级变更同步学习报告记录到Redis结束");
                }
            }
        }

    }
}