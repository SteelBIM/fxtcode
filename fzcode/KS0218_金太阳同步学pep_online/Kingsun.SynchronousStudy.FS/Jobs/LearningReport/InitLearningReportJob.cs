using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.IBS.BLL.IBSLearningReport;
using Kingsun.IBS.IBLL.IBSLearningReport;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class InitLearningReportJob:TimedTask,IJob
    {
        private static IIBSLearningReport report = new IBSLearningReportBLL();
        int TypeName = Convert.ToInt32(ConfigurationManager.AppSettings["TypeName"]);

        string kingsun_ot = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_ot"].ConnectionString;
        string kingsun_bj = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
        string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_sz"].ConnectionString;
        string kingsun_rj = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_rjpep"].ConnectionString;
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

                    switch (TypeName)
                    {
                        #region 深圳
                        case 1:
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis开始");

                            report.InitLearningReport(kingsun_sz);
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis结束");
                            break;
                        case 2:
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis开始");
                            report.InitLearningReportModuleTitle(kingsun_sz);
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis结束");
                            break;
                        case 3:
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis开始");
                            report.InitLearningReportBookCatalogues(kingsun_sz);
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis结束");
                            break;
                        case 4:
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis开始");
                            report.InitLearningReportUserInfo(kingsun_sz);
                            Log4Net.LogHelper.Info("初始化SZ学习报告记录到Redis结束");
                            break;
                        #endregion

                        #region 北京
                        case 5:
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis开始");
                            report.InitLearningReport(kingsun_bj);
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis结束");
                            break;
                        case 6:
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis开始");
                            report.InitLearningReportModuleTitle(kingsun_bj);
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis结束");
                            break;
                        case 7:
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis开始");
                            report.InitLearningReportBookCatalogues(kingsun_bj);
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis结束");
                            break;
                        case 8:
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis开始");
                            report.InitLearningReportUserInfo(kingsun_bj);
                            Log4Net.LogHelper.Info("初始化BJ学习报告记录到Redis结束");
                            break;
                        #endregion

                        #region 其他
                        case 9:
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis开始");
                            report.InitLearningReport(kingsun_ot);
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis结束");
                            break;
                        case 10:
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis开始");
                            report.InitLearningReportModuleTitle(kingsun_ot);
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis结束");
                            break;
                        case 11:
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis开始");
                            report.InitLearningReportBookCatalogues(kingsun_ot);
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis结束");
                            break;
                        case 12:
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis开始");
                            report.InitLearningReportUserInfo(kingsun_ot);
                            Log4Net.LogHelper.Info("初始化OT学习报告记录到Redis结束");
                            break;
                        #endregion

                        #region 人教优学
                        case 13:
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis开始");
                            report.InitLearningReport(kingsun_rj);
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis结束");
                            break;
                        case 14:
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis开始");
                            report.InitLearningReportModuleTitle(kingsun_rj);
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis结束");
                            break;
                        case 15:
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis开始");
                            report.InitLearningReportBookCatalogues(kingsun_rj);
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis结束");
                            break;
                        case 16:
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis开始");
                            report.InitLearningReportUserInfo(kingsun_rj);
                            Log4Net.LogHelper.Info("初始化RJ学习报告记录到Redis结束");
                            break;
                        #endregion
                    }
                }
            }
        }
    }
}