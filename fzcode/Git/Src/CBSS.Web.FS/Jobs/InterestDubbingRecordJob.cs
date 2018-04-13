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
using CBSS.Tbx.IBLL;
using CBSS.Tbx.BLL;

namespace CourseActivate.Web.FS.Jobs
{
    /// <summary>
    /// 趣配音记录
    /// </summary>
    public class InterestDubbingRecordJob
    {
        IIBSService ibsService = new IBSService();
        ITbxService tbxService = new TbxService();

        public void ExcutedInterestingRank()
        {
            lock (TimedTask._rankInfoLock)
            {
                try
                {
                    tbxService.ExcutedInterestingRank();
                    // ibsService.GetDubbingByCataId(13989);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "同步排行榜数据异常", ex);
                }

            }
        }
    }

}