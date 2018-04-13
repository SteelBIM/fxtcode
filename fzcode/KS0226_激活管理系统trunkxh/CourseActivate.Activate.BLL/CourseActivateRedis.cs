using CourseActivate.Activate.Constract.Model;
using CourseActivate.Activate.DAL;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.BLL
{
    public class CourseActivateRedis : DoRedisString
    {
        public void InitRedisData()
        {
            //ClearRedisData();
            ActivateCourseDAL manager = new ActivateCourseDAL();
            List<tb_batch> blist = manager.SelectAll<tb_batch>();

            List<tb_activatetype> atlist = manager.SelectAll<tb_activatetype>();
            List<tb_batchactivateuse> baulsit = manager.SelectAll<tb_batchactivateuse>();
            Dictionary<string, tb_batch> dic = new Dictionary<string, tb_batch>();
            for (int i = 0; i < blist.Count; i++)
            {
                dic.Add(RedisConfiguration.BatchKey + blist[i].batchid.ToString(), blist[i]);
            }
            base.Set<tb_batch>(dic);
            Dictionary<string, tb_batchactivate> dic2 = new Dictionary<string, tb_batchactivate>();
            ActivateCourseDAL dal = new ActivateCourseDAL();
            for (int i = 0; i < blist.Count; i++)
            {
                dal.InitBatchRedis(blist[i].batchid);
            }
            Dictionary<string, tb_activatetype> dic3 = new Dictionary<string, tb_activatetype>();
            for (int i = 0; i < atlist.Count; i++)
            {
                dic3.Add(RedisConfiguration.ActivateTypeKey + atlist[i].activatetypeid, atlist[i]);
            }
            base.Set<tb_activatetype>(dic3);

            Dictionary<string, tb_batchactivateuse> dic4 = new Dictionary<string, tb_batchactivateuse>();
            for (int i = 0; i < baulsit.Count; i++)
            {
                dic4.Add(RedisConfiguration.ActivateUseKey + baulsit[i].activateid, baulsit[i]);
            }
            base.Set<tb_batchactivateuse>(dic4);
        }

        public void ClearRedisData()
        {
            base.FlushDB();
        }

    }
}
