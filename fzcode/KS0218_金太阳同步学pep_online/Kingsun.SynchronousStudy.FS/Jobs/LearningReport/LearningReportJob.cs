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
    public class LearningReportJob:IJob
    {
        static IIBSLearningReport report = new IBSLearningReportBLL();
        void IJob.Execute()
        {
            lock (TimedTask._LearningReport)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("MOD2IBS同步开始");
                    report.ExecuteLearningReport();
                    Log4Net.LogHelper.Info("MOD2IBS同步结束");
                }
            }
        }

    }
}