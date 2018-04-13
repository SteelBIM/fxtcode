using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class VideoInfoBLL
    {
        VideoInfoDAL videoDAL = new VideoInfoDAL();
        static RedisListHelper ibsRedisList = new RedisListHelper();


        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public void InsertVideoInfoRedis()
        {
            var listCount = ibsRedisList.Count("RankQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = ibsRedisList.RemoveStartFromList("RankQueue");
                if (!string.IsNullOrEmpty(model))
                {
                    RedisVideoInfo data = JsonHelper.DecodeJson<RedisVideoInfo>(model);
                    videoDAL.InsertVideoRedis(data);

                }
            }
        }

        public void insertBJVideoInfo()
        {
            videoDAL.insertBJVideoInfo();
        }

        public void insertSZVideoInfo()
        {
            videoDAL.insertSZVideoInfo();
        }

        public void insertOTVideoInfo()
        {
            videoDAL.insertOTVideoInfo();
        }

        public void insertBJNewRank()
        {
            videoDAL.insertBJNewRank();
        }

        public void insertBJSchoolRank()
        {
            videoDAL.insertBJSchoolRank();
        }

        public void insertBJClassRank()
        {
            videoDAL.insertBJClassRank();
        }

        public void insertSZNewRank()
        {
            videoDAL.insertSZNewRank();
        }

        public void insertSZSchoolRank()
        {
            videoDAL.insertSZSchoolRank();
        }

        public void insertSZClassRank()
        {
            videoDAL.insertSZClassRank();
        }

        public void insertOTNewRank()
        {
            videoDAL.insertOTNewRank();
        }

        public void insertOTSchoolRank()
        {
            videoDAL.insertOTSchoolRank();
        }

        public void insertOTClassRank()
        {
            videoDAL.insertOTClassRank();
        }
        
    }
}
