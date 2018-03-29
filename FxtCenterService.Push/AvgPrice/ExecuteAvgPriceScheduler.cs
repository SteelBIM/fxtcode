using CAS.Common;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Push.AvgPrice
{
    /// <summary>
    /// 预约执行触发器
    /// zhoub 20160316
    /// </summary>
    public class ExecuteAvgPriceScheduler
    {
        private static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public static void Start()
        {
            LogHelper.Info("启动推送功能");

            scheduler.Start();
            IJobDetail job = JobBuilder.Create<ExecuteAvgPrice>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("ExecuteAvgPrice", "OM").WithSimpleSchedule(t => t.WithIntervalInSeconds(300).RepeatForever()).Build();
            //ITrigger trigger = TriggerBuilder.Create().WithIdentity("ExecuteAvgPrice", "OM").WithCronSchedule("0 0 0 10 * ?").Build();
            scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// 暂停调度中所有的job任务
        /// zhoub 20160316
        /// </summary>
        public static void PauseAll()
        {
            scheduler.PauseAll();
        }

        /// <summary>
        /// 恢复调度中所有的job的任务
        /// zhoub 20160316
        /// </summary>
        public static void ResumeAll()
        {
            scheduler.ResumeAll();
        }
    }
}
