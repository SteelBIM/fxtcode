using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.ExamPaper.BLL;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class InitExampaperModelJob:IJob
    {
        int ExamPaperTypeName = Convert.ToInt32(ConfigurationManager.AppSettings["ExamPaperTypeName"]);

        string kingsun_ot = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_ot"].ConnectionString;
        string kingsun_bj = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
        string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_sz"].ConnectionString;
        string kingsun_rj = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_rjpep"].ConnectionString;
        void IJob.Execute()
        {
            lock (TimedTask.InitLock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    switch (ExamPaperTypeName)
                    {
                        case 1://深圳
                            Log4Net.LogHelper.Info("初始化单元测试数据SZ开始");
                            new StuCatalogBLL().GetAllStuAnswer(kingsun_sz);
                            Log4Net.LogHelper.Info("初始化单元测试数据SZ结束");
                            break;
                        case 2://北京
                            Log4Net.LogHelper.Info("初始化单元测试数据BJ开始");
                            new StuCatalogBLL().GetAllStuAnswer(kingsun_bj);
                            Log4Net.LogHelper.Info("初始化单元测试数据BJ结束");
                            break;
                        case 3://ot
                            Log4Net.LogHelper.Info("初始化单元测试数据OT开始");
                            new StuCatalogBLL().GetAllStuAnswer(kingsun_ot);
                            Log4Net.LogHelper.Info("初始化单元测试数据OT结束");
                            break;
                        case 4://人教
                            Log4Net.LogHelper.Info("初始化单元测试数据RJ开始");
                            new StuCatalogBLL().GetAllStuAnswer(kingsun_rj);
                            Log4Net.LogHelper.Info("初始化单元测试数据RJ开始");
                            break;

                    }
                }
            }
         

        }
    }
}