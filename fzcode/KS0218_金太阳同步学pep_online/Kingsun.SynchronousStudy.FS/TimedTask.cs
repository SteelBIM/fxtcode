using FluentScheduler;
using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common.Base;
using System.Reflection;
using Kingsun.SynchronousStudy.BLL;
using System.Configuration;
using Kingsun.ExamPaper.BLL;
using Kingsun.Fs.Jobs;
using Kingsun.Fs.Jobs.IBS2MOD;
using Kingsun.InterestDubbingGame.BLL;
using Kingsun.IBS.BLL.MOD2IBS;
using Kingsun.IBS.IBLL.IBS_MOD;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL.IBSLearningReport;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.IBS.BLL.IBSLearningReport;
using ServiceStack.Text;

namespace Kingsun.SynchronousStudy.FS
{
    public class TimedTask : IJob, IRegisteredObject
    {
        public static readonly object _lock = new object();
        public static readonly object _ibs2modlock = new object();
        public static readonly object _mod2ibs = new object();
        public static readonly object _lock_UserInfo = new object();
        public static readonly object _lock_ClassInfo = new object();
        public static readonly object _lock_SchInfo = new object();
        public static readonly object _lock_AreaInfo = new object();
        public static readonly object _UpdateUserSchoolRankList = new object();

        public static readonly object _vaillock = new object();

        public static readonly object _LearningReport = new object();
        public static readonly object _TodayLearningReport = new object();

        public static readonly  object _repairlock=new object();

        public static readonly  object _lock_Order2Base=new object();

        public static readonly object _classChangeLearningReport = new object();

        public static readonly object ExamPaper2DBLock = new object();

        /// <summary>
        /// 所有初始化的锁
        /// </summary>
        public static readonly object InitLock = new object();

        public static bool _shuttingDown=false;
        private static IIBSLearningReport report = new IBSLearningReportBLL();

        public Registry Start()
        {
            VailUserJob job = new VailUserJob();
            job.VailUser();
            HostingEnvironment.RegisterObject(this);
            Registry registry = new Registry();
            try
            {
                //同步趣配音报名用户信息
                int SyncDubbing = Convert.ToInt32(ConfigurationManager.AppSettings["SyncDubbing"]);
                //同步趣配音报名用户名称
                int SyncUserInfoRedisFromUMS = Convert.ToInt32(ConfigurationManager.AppSettings["SyncUserInfoRedisFromUMS"]);
                //刷新学生所在学校排行榜
                int UpdateUserSchoolRankList = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateUserSchoolRankList"]);


                //初始化数据到Redis
                int StartMOD2IBSFirst = Convert.ToInt32(ConfigurationManager.AppSettings["StartMOD2IBSFirst"]);
                int FirstCreateDataInteval = Convert.ToInt32(ConfigurationManager.AppSettings["FirstCreateDataInteval"]);

                //MOD2IBS同步
                int StartMOD2IBS = Convert.ToInt32(ConfigurationManager.AppSettings["StartMOD2IBS"]);
                int MOD2IBSInterval = Convert.ToInt32(ConfigurationManager.AppSettings["MOD2IBSInterval"]);


                //修复IBS
                int StartRepairIBS = Convert.ToInt32(ConfigurationManager.AppSettings["StartRepairIBS"]);

                int SynVideoRank = Convert.ToInt32(ConfigurationManager.AppSettings["SynVideoRank"]);

                int IsRemove = Convert.ToInt32(ConfigurationManager.AppSettings["IsRemove"]);

                //学习报告同步
                int LearningReportInterval = Convert.ToInt32(ConfigurationManager.AppSettings["LearningReportInterval"]);
                int StartLearningReport = Convert.ToInt32(ConfigurationManager.AppSettings["StartLearningReport"]);

                //学习报告初始化
                int StartTodayInitLearningReport = Convert.ToInt32(ConfigurationManager.AppSettings["StartTodayInitLearningReport"]);
                int InitLearningReportInterval = Convert.ToInt32(ConfigurationManager.AppSettings["InitLearningReportInterval"]);
                int StartInitLearningReport = Convert.ToInt32(ConfigurationManager.AppSettings["StartInitLearningReport"]);
                
                //单元测试数据入库
                int Exampaper2DbModelInterval = Convert.ToInt32(ConfigurationManager.AppSettings["Exampaper2DbModelInterval"]);
                int StartExampaper2DbModel = Convert.ToInt32(ConfigurationManager.AppSettings["StartExampaper2DbModel"]);
                int StartInitExampaper2DbModel = Convert.ToInt32(ConfigurationManager.AppSettings["StartInitExampaper2DbModel"]);

                //初始化那个区域


                registry.Schedule(() => Execute()).ToRunEvery(1).Days().At(1, 00);

                //有效用户
                registry.Schedule<VailUserJob>().ToRunEvery(1).Days().At(1, 00);


                int startOrderInfoJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartOrderInfoJob"]);
                if (startOrderInfoJob == 1)
                {

                     registry.Schedule<SyncOrderInfo2BaseDBJob>().ToRunNow().AndEvery(5).Seconds();//同步订单到总库
                }

                /*int startUserInfoJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartUserInfoJob"]);
                if (startUserInfoJob == 1)
                {
                    // registry.Schedule<SyncUserInfoJob>().ToRunEvery(1).Hours();//同步用户到总库
                }*/



                int startDubbingDataJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartDubbingDataJob"]);
                if (startDubbingDataJob == 1)
                {
                    registry.Schedule<SyncDubbingDataJob>().ToRunEvery(SyncDubbing).Minutes();//同步趣配音报名用户信息
                }

                int startUserInfoRedisFromUMSJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartUserInfoRedisFromUMSJob"]);
                if (startUserInfoRedisFromUMSJob == 1)
                {
                    registry.Schedule<SyncUserInfoRedisFromUMSJob>().ToRunEvery(SyncUserInfoRedisFromUMS).Minutes();//同步趣配音报名用户名称
                }

                int startSchoolRankListJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartSchoolRankListJob"]);
                if (startSchoolRankListJob == 1)
                {
                    registry.Schedule<UpdateUserSchoolRankListJob>().ToRunEvery(UpdateUserSchoolRankList).Minutes();//刷新学生所在学校排行榜redis数据
                }


                int startRankListJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartRankListJob"]);
                if (startRankListJob == 1)
                {
                    //趣配音排行榜定时任务
                    registry.Schedule<VideoInfoRankListJob>().ToRunEvery(SynVideoRank).Minutes();
                }

                /*int StartIBS2MODJob = Convert.ToInt32(ConfigurationManager.AppSettings["StartIBS2MODJob"]);
                int IBSRegisterUser2MODInteval = Convert.ToInt32(ConfigurationManager.AppSettings["IBSRegisterUser2MODInteval"]);
                if (StartIBS2MODJob == 1)
                {
                    registry.Schedule<AppLoginRegisterJob>().ToRunNow().AndEvery(IBSRegisterUser2MODInteval).Minutes();
                    registry.Schedule<OneMinRetryAppLoginRegisterJob>().ToRunEvery(1).Minutes();
                    registry.Schedule<TenMinRetryAppLoginRegisterJob>().ToRunEvery(10).Minutes();
                }*/

                ///初始化同步开关
                if (StartMOD2IBSFirst == 1)
                {
                    //正式
                    registry.Schedule<MOD2IBSUserInfoJob>().ToRunNow().AndEvery(FirstCreateDataInteval).Years();
                    registry.Schedule<MOD2IBSClassInfoJob>().ToRunNow().AndEvery(FirstCreateDataInteval).Years();
                    registry.Schedule<MOD2IBSSchInfoJob>().ToRunNow().AndEvery(FirstCreateDataInteval).Years();
                    registry.Schedule<MOD2IBSAreaInfoJob>().ToRunNow().AndEvery(FirstCreateDataInteval).Years();
                }

                if (StartRepairIBS == 1)
                {
                    registry.Schedule<IBSRepairJob>().ToRunNow();
                }



                ///MOD同步开关
                if (StartMOD2IBS == 1)
                {
                    registry.Schedule<MOD2IBSJob>().ToRunEvery(MOD2IBSInterval).Seconds();
                }


                //同步学习报告
                if (StartLearningReport == 1)
                {
                    registry.Schedule<LearningReportJob>().ToRunNow().AndEvery(LearningReportInterval).Seconds();
                    registry.Schedule<StudentClassChangeLearningReportJob>()
                        .ToRunNow()
                        .AndEvery(LearningReportInterval)
                        .Seconds();
                }

                //学习报告初始化
                if (StartInitLearningReport == 1)
                {
                    registry.Schedule<InitLearningReportJob>().ToRunNow().AndEvery(InitLearningReportInterval).Years();
                }
                //特定时间之后的初始化
                if (StartTodayInitLearningReport == 1)
                {
                    registry.Schedule<InitTodayLearningReportJob>().ToRunNow().AndEvery(InitLearningReportInterval).Years();
                }
                //删除学习报告的Redis表
                if (IsRemove == 1)
                {
                    lock (_classChangeLearningReport)
                    {
                        report.RemoveRedis();
                    }
                }

                //单元测试入库同步
                if (StartExampaper2DbModel == 1)
                {
                    registry.Schedule<OTExampaper2DbModelJob>().ToRunNow().AndEvery(Exampaper2DbModelInterval).Seconds();
                    registry.Schedule<SZExampaper2DbModelJob>().ToRunNow().AndEvery(Exampaper2DbModelInterval).Seconds();
                    registry.Schedule<BJExampaper2DbModelJob>().ToRunNow().AndEvery(Exampaper2DbModelInterval).Seconds();
                    registry.Schedule<RJExampaper2DbModelJob>().ToRunNow().AndEvery(Exampaper2DbModelInterval).Seconds();
                }

                ///单元测试初始化
                if (StartInitExampaper2DbModel == 1)
                {
                    registry.Schedule<InitExampaperModelJob>().ToRunNow().AndEvery(Exampaper2DbModelInterval).Years();
                }


            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "定时任务启动异常");
            }


            return registry;
        }



        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("定时任务正常执行");
                }
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }


    }
    public class SchoolInfoModel
    {
        public string DistrictID { get; set; }
        public string Area { get; set; }
        public string SchoolName { get; set; }
    }
}