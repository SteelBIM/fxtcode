using CBSS.Core.Log;
using CBSS.Framework.Redis;
using CourseActivate.Web.FS.Jobs;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using CBSS.Core.Utility;

namespace CourseActivate.Web.FS.Controllers
{

    /// <summary>
    /// TimedTask 的摘要说明
    /// </summary>
    public class TimedTask : IJob, IRegisteredObject
    {
        static string filepath = "Config/SettingConfig.xml";
        public static readonly object _lock = new object();
        public static readonly object _JuniorEnglishRecordlock = new object();//初中英语学习记录入库
        public static readonly object _JuniorEnglishSpokenRecordock = new object();//初中英语口讯记录入库
        public static readonly object _SyncMODResourcelock = new object();//获取新MOD同步教材资源
        public static readonly object _SpokenPaperRecordlock = new object();//初中英语口讯记录入库
        public static readonly object _SyncYXOrderLock = new object();
        public static readonly object _LearningReportLock = new object();
        public static readonly object _classChangeLock = new object();
        public static readonly object _rankInfoLock = new object();
        public static bool _shuttingDown;

        public Registry Start()
        {
            HostingEnvironment.RegisterObject(this);
            Registry registry = new Registry();
            registry.Schedule(() => Execute()).ToRunEvery(1).Days().At(14, 45);//每天几点执行一次代码

            #region 同步新MOD资源
            int SyncMODResourceSwitch = Convert.ToInt32(XMLHelper.GetAppSetting("SyncMODResourceSwitch"));//开关
            int SyncMODResourceInteval = Convert.ToInt32(XMLHelper.GetAppSetting("SyncMODResourceInteval"));//间隔
            if (SyncMODResourceSwitch == 1)
            {
                registry.Schedule(() => new IBSResourceJob().SyncMODResourceExecute()).WithName("SyncMODResourceExecute").ToRunNow().AndEvery(SyncMODResourceInteval).Minutes();
            }
            #endregion

            #region 用户记录
            //初中英语学习记录入库
            int JuniorEnglishRecordSwitch = Convert.ToInt32(XMLHelper.GetAppSetting(filepath,"Switchs","JuniorEnglishRecordSwitch"));
            int JuniorEnglishRecordInteval = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Intevals", "JuniorEnglishRecordInteval"));
            if (JuniorEnglishRecordSwitch == 1)
            {
      
                registry.Schedule(() => new JuniorEnglishRecordJob().JuniorEnglishRecordExecute()).WithName("JuniorEnglishRecordExecute").ToRunNow().AndEvery(JuniorEnglishRecordInteval).Seconds();
            }
            //初中英语口训模块学习记录入库
            int JuniorEnglishSpokenRecordSwitch = Convert.ToInt32(XMLHelper.GetAppSetting(filepath,"Switchs","JuniorEnglishSpokenRecordSwitch"));
            int JuniorEnglishSpokenRecordInteval = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Intevals", "JuniorEnglishSpokenRecordInteval"));
            if (JuniorEnglishRecordSwitch == 1)
            {
                registry.Schedule(() => new JuniorEnglishRecordJob().JuniorEnglishSpokenRecordExecute()).WithName("JuniorEnglishSpokenRecordExecute").ToRunNow().AndEvery(JuniorEnglishSpokenRecordInteval).Seconds();
            }
            #endregion

            #region 小学英语同步学习报告
            int PrimaryLearningReportSwitch = Convert.ToInt32(XMLHelper.GetAppSetting(filepath,"Switchs","PrimaryLearningReportSwitch"));
            int PrimaryLearningReportInteval = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Intevals", "PrimaryLearningReportInteval"));
            if (PrimaryLearningReportSwitch == 1)
            {
                registry.Schedule(() => new LearningReportJob().ExcutedLearningReport()).WithName("PrimaryLearningReportExecute").ToRunNow().AndEvery(PrimaryLearningReportInteval).Seconds();
            }
            #endregion

            #region 小学英语班级变更后学习报告随着变更
            int PrimaryClassChangeLearningReportSwitch = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Switchs", "PrimaryClassChangeLearningReportSwitch"));
            int PrimaryClassChangeLearningReportInteval = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Intevals", "PrimaryClassChangeLearningReportInteval"));
            if (PrimaryClassChangeLearningReportSwitch == 1)
            {
                registry.Schedule(() => new LearningReportJob().ExcutedLearningReport()).WithName("PrimaryClassChangeLearningReportExecute").ToRunNow().AndEvery(PrimaryClassChangeLearningReportInteval).Seconds();
            }
            #endregion
            #region 排行版
            int RankInfoSwitch = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Switchs", "RankInfoSwitch"));
            int RankInfoInteval = Convert.ToInt32(XMLHelper.GetAppSetting(filepath, "Intevals", "RankInfoInteval"));
            if (RankInfoSwitch == 1)
            {
                registry.Schedule(() => new InterestDubbingRecordJob().ExcutedInterestingRank()).WithName("InterestingRankExecute").ToRunNow().AndEvery(RankInfoInteval).Minutes();
            }
            #endregion
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
                    Log4NetHelper.Info(LoggerType.FsExceptionLog, "定时任务正常执行");
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
}
