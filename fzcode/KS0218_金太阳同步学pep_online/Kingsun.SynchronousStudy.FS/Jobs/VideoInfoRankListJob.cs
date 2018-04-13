using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.FS;

namespace Kingsun.Fs.Jobs
{
    public class VideoInfoRankListJob:IJob
    {
        public void Execute()
        {
            lock (TimedTask._UpdateUserSchoolRankList)
            {
                if (TimedTask._shuttingDown)
                {
                    return;
                }
                else
                {
                    Log4Net.LogHelper.Info("刷新课本剧排行榜redis数据开始");
                    new VideoInfoBLL().InsertVideoInfoRedis();
                    Log4Net.LogHelper.Info("刷新课本剧排行榜redis数据结束");
                }
            }
        }
    }
}