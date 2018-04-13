using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.InterestDubbingGame.BLL;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class UpdateUserSchoolRankListJob:IJob
    {
        public void Execute()
        {
            lock (TimedTask._lock)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("刷新学生所在学校排行榜redis数据开始");
                    new TB_InterestDubbingGame_MatchBLL().UpdateUserSchoolRankList();
                    Log4Net.LogHelper.Info("刷新学生所在学校排行榜redis数据结束");
                }
            }
        }
    }
}