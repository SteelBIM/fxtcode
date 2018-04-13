using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Framework.DAL;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;

namespace CourseActivate.Activate.DAL
{
    public class ActivateCourseDAL : Manage
    {
        public tb_batch GetBatchInfo(int batchid)
        {
            tb_batch binfo = redis.Get<tb_batch>(RedisConfiguration.BatchKey + batchid.ToString());
            if (binfo == null)
            {
                binfo = Select<tb_batch>(batchid);
                if (binfo != null)
                {
                    redis.Set<tb_batch>(RedisConfiguration.BatchKey + batchid.ToString(), binfo);
                }
            }
            return binfo;
        }

        /// <summary>
        /// 获取所有的批次信息
        /// </summary>
        /// <returns></returns>
        public List<tb_batch> GetBatchList()
        {
            return SelectAll<tb_batch>();
        }

        DoRedisString redis = new DoRedisString();
        /// <summary>
        /// 获取激活码信息，不存在返回空
        /// </summary>
        /// <param name="activatecode"></param>
        /// <returns></returns>
        public tb_batchactivate GetBatchActivate(string activatecode)
        {
            tb_batchactivate bainfo = redis.Get<tb_batchactivate>(RedisConfiguration.ActivateKey + activatecode);
            if (bainfo == null)
            {
                bainfo = SelectSearch<tb_batchactivate>(i => i.activatecode == activatecode).FirstOrDefault();
                if (bainfo != null)
                {
                    redis.Set<tb_batchactivate>(RedisConfiguration.ActivateKey + activatecode, bainfo);
                }
            }
            return bainfo;
        }

        /// <summary>
        /// 获取激活码使用记录
        /// </summary>
        /// <param name="acitvateid"></param>
        /// <returns></returns>
        public tb_batchactivateuse GetActivateCodeUse(int acitvateid)
        {
            tb_batchactivateuse bauinfo = redis.Get<tb_batchactivateuse>(RedisConfiguration.ActivateUseKey + acitvateid.ToString());
            if (bauinfo == null)
            {
                bauinfo = SelectSearch<tb_batchactivateuse>(i => i.activateid == acitvateid).FirstOrDefault();
                if (bauinfo != null)
                {
                    redis.Set<tb_batchactivateuse>(RedisConfiguration.ActivateUseKey + acitvateid.ToString(), bauinfo);
                }
            }
            return bauinfo;
        }

        /// <summary>
        /// 获取缓存中的激活码使用设备列表
        /// </summary>
        /// <param name="activateuseid"></param>
        /// <returns></returns>
        public List<tb_batchactivateusedevice> GetActivateCodeDeviceList(Guid activateuseid)
        {
            // return redis.GetAll<tb_batchactivateusedevice>(RedisConfiguration.ActivateUseDeviceKey + activateuseid.ToString() + "_*");

            List<tb_batchactivateusedevice> list = redis.Get<List<tb_batchactivateusedevice>>(RedisConfiguration.ActivateUseDeviceKey + activateuseid);
            if (list == null || list.Count == 0)
            {
                list = SelectSearch<tb_batchactivateusedevice>(i => i.activateuseid == activateuseid);
                if (list != null && list.Count != 0)
                {
                    redis.Set<List<tb_batchactivateusedevice>>(RedisConfiguration.ActivateUseDeviceKey + activateuseid, list);
                }
            }
            return list;
            // return SelectSearch<tb_batchactivateusedevice>(i => i.activateuseid == activateuseid);
        }

        /// <summary>
        /// 获取批次书本信息
        /// </summary>
        /// <param name="batchid"></param>
        /// <returns></returns>
        public tb_batchbooks GetBatchBookInfo(int batchid)
        {
            tb_batchbooks bbinfo = redis.Get<tb_batchbooks>(RedisConfiguration.BatchBookKey + batchid);
            if (bbinfo == null)
            {
                bbinfo = SelectSearch<tb_batchbooks>(i => i.batchid == batchid).FirstOrDefault();
                if (bbinfo != null)
                {
                    redis.Set<tb_batchbooks>(RedisConfiguration.BatchBookKey + batchid, bbinfo);
                }
            }
            return bbinfo;
        }

        /// <summary>
        /// 获取激活码类型信息
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public tb_activatetype GetActivateType(int typeid)
        {
            tb_activatetype atinfo = redis.Get<tb_activatetype>(RedisConfiguration.ActivateTypeKey + typeid.ToString());
            if (atinfo == null)
            {
                atinfo = base.Select<tb_activatetype>(typeid);
                if (atinfo != null)
                {
                    redis.Set<tb_activatetype>(RedisConfiguration.ActivateTypeKey + typeid.ToString(), atinfo);
                }
            }
            return atinfo;
        }

        /// <summary>
        /// 改变批次的状态
        /// </summary>
        /// <param name="batchid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool SetBatch(int batchid, int status)
        {
            tb_batch binfo = GetBatchInfo(batchid);
            if (binfo == null)
                return false;
            binfo.status = status;
            return redis.Set<tb_batch>(RedisConfiguration.BatchKey + binfo.batchid.ToString(), binfo);
        }


        /// <summary>
        /// 设置redis中批次的信息
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public bool SetBatch(tb_batch batch)
        {
            return redis.Set<tb_batch>(RedisConfiguration.BatchKey + batch.batchid.ToString(), batch);
        }

        public object GetRedis(string key)
        {
            return redis.GetAll<string>(key);
        }

        public object GetRedisObj(string key)
        {
            return redis.Get<object>(key);
        }

        public bool SetRedis<T>(string key, T value)
        {
            return redis.Set<T>(key, value);
        }

        public T GetRedis<T>(string key)
        {
            return redis.Get<T>(key);
        }

        public List<string> GetList(string key)
        {
            return new DoRedisList().Get(key);
        }

        public bool RemoveRedis(string Key)
        {
            return redis.Remove(Key);
        }

        /// <summary>
        /// 批量移除redis
        /// </summary>
        /// <param name="Key"></param>
        public void RemoveRedis(List<string> Key)
        {
            int count = Key.Count;
            int mcount = 50000;
            if (count > mcount)
            {
                int m = count / mcount;
                int y = count % mcount;
                if (y != 0)
                    m++;
                for (int j = 0; j < m; j++)
                {
                    int tmp = mcount;
                    if ((j + 1) == m && y != 0)
                    {
                        tmp = y;
                    }
                    List<string> tmplist = new List<string>();
                    for (int i = mcount * j; i < tmp * (j + 1); i++)
                    {
                        tmplist.Add(Key[i]);
                    }
                    redis.Remove(tmplist);
                    // TestLog4Net.LogHelper.Info("激活码删除redis数据，操作结果：未知。参数信息:无。删除时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "。分多次删除，每次" + mcount + "条数据,保存轮次：" + j);
                }
            }
            else
            {
                redis.Remove(Key);
                //TestLog4Net.LogHelper.Info("激活码删除redis数据，操作结果：未知。参数信息:无。删除时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "。" + mcount + "条数据。");
            }
        }


        /// <summary>
        /// redis添加激活码使用记录
        /// </summary>
        /// <param name="useinfo"></param>
        /// <returns></returns>
        public bool AddActivateUse(tb_batchactivateuse useinfo)
        {
            tb_batchactivateuse bauinfo = redis.Get<tb_batchactivateuse>(RedisConfiguration.GetActivateUseKey(useinfo.activateid.Value));
            if (bauinfo == null)
            {
                #region tb_batchactivateuseDaily记录每天激活的激活码.用于统计,统计完成会清空
                DoRedisList listredis = new DoRedisList();
                listredis.LPush(RedisConfiguration.GetActivateStatiscKey("Daily" + DateTime.Now.ToString("yyyyMMdd")), useinfo.bookid.Value.ToString());

                //var tb_batchactivateuseDaily = redis.Get<List<tb_batchactivateuse>>(RedisConfiguration.ActivateStatiscKey + "Daily");
                //if (tb_batchactivateuseDaily == null)
                //{
                //    tb_batchactivateuseDaily = new List<tb_batchactivateuse>();
                //}
                //tb_batchactivateuseDaily.Add(useinfo);

                //redis.Set<List<tb_batchactivateuse>>(RedisConfiguration.ActivateStatiscKey + "Daily", tb_batchactivateuseDaily);

                #endregion
                bool b = redis.Set<tb_batchactivateuse>(RedisConfiguration.GetActivateUseKey(useinfo.activateid.Value), useinfo);
                //TestLog4Net.LogHelper.Info("激活码使用记录保存到redis，保存结果：" + b + "参数信息:" + JsonHelper.EncodeJson(useinfo));
                return b;
            }
            return true;
        }

        /// <summary>
        /// redis添加激活码使用设备记录
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool AddUseDevice(tb_batchactivateusedevice device)
        {
            List<tb_batchactivateusedevice> list = redis.Get<List<tb_batchactivateusedevice>>(RedisConfiguration.ActivateUseDeviceKey + device.activateuseid);
            //List<tb_batchactivateusedevice> list = GetActivateCodeDeviceList(device.activateuseid.Value);
            if (list == null)
            {
                list = new List<tb_batchactivateusedevice>();
            }
            list.Add(device);
            bool b = redis.Set<List<tb_batchactivateusedevice>>(RedisConfiguration.ActivateUseDeviceKey + device.activateuseid, list);
            return b;
        }

        /// <summary>
        /// 同步某一批次激活码到redis
        /// </summary>
        /// <param name="batchid"></param>
        public void InitBatchRedis(int batchid)
        {
            ActivateCourseDAL manager = new ActivateCourseDAL();
            tb_batch binfo = manager.Select<tb_batch>(batchid);
            if (binfo != null)
            {
                redis.Set<tb_batch>(RedisConfiguration.BatchKey + batchid, binfo);
                List<tb_batchactivate> balist = manager.SelectSearch<tb_batchactivate>(i => i.batchid == batchid);
                if (balist != null)
                {
                    int count = balist.Count;
                    int mcount = 50000;
                    if (count > mcount)
                    {
                        int m = count / mcount;
                        int y = count % mcount;
                        if (y != 0)
                            m++;
                        for (int j = 0; j < m; j++)
                        {
                            int tmp = mcount;
                            if ((j + 1) == m && y != 0)
                            {
                                tmp = y;
                            }
                            Dictionary<string, tb_batchactivate> dic2 = new Dictionary<string, tb_batchactivate>();
                            for (int i = mcount * j; i < (mcount * j + tmp); i++)
                            {
                                dic2.Add(RedisConfiguration.ActivateKey + balist[i].activatecode, balist[i]);
                            }
                            if (dic2.Count == 0)
                            {
                                return;
                            }
                            redis.Set<tb_batchactivate>(dic2);
                            //TestLog4Net.LogHelper.Info("激活码保存到redis，保存结果：未知。参数信息:无。保存批次：" + batchid + "。分多次保存到redis，每次" + mcount + "条数据,保存轮次：" + j);
                        }
                    }
                    else
                    {
                        Dictionary<string, tb_batchactivate> dic2 = new Dictionary<string, tb_batchactivate>();
                        for (int i = 0; i < balist.Count; i++)
                        {
                            dic2.Add(RedisConfiguration.ActivateKey + balist[i].activatecode, balist[i]);
                        }
                        if (dic2.Count == 0)
                        {
                            return;
                        }
                        redis.Set<tb_batchactivate>(dic2);
                        //TestLog4Net.LogHelper.Info("激活码保存到redis，保存结果：未知。参数信息:无。保存批次：" + batchid + "。" + mcount + "条数据。");
                    }
                }
            }
            new Framework.DAL.DoRedisHash().RemoveEntryFromHash("AddActivateCodeRange", batchid.ToString());
        }

        /// <summary>
        /// redis添加激活码使用类型
        /// </summary>
        /// <param name="typeinfo"></param>
        /// <returns></returns>
        public bool AddActivateType(tb_activatetype typeinfo)
        {
            return redis.Set<tb_activatetype>(RedisConfiguration.ActivateTypeKey + typeinfo.activatetypeid, typeinfo);
        }

        /// <summary>
        /// 激活记录写入数据库
        /// </summary>
        /// <param name="typeinfo"></param>
        /// <returns></returns>
        public void AddActivateRecord(tb_batchactivateuse useinfo, tb_batchactivateusedevice device, int batchid = 0)
        {
            TestLog4Net.LogHelper.Info(useinfo.activatecode + "-Out Rang Start");
            if (useinfo != null)
            {
                //List<RepositoryAction> actions = new List<RepositoryAction>();
                //RepositoryAction ac1 = new RepositoryAction();
                //ac1.Entity = useinfo;
                //ac1.Actions = Acitons.Insert;
                //actions.Add(ac1);

                //RepositoryAction ac2 = new RepositoryAction();
                //ac2.Actions = Acitons.Insert;
                //ac2.Entity = device;
                //actions.Add(ac2);

                //bool b = TransactionOperate(actions);


                if (Insert<tb_batchactivateuse>(useinfo) != null)
                {
                    //if (Insert<tb_batchactivateusedevice>(device) == null)
                    //{
                    //    TestLog4Net.LogHelper.Info("Error:激活码使用设备记录数据库失败。参数信息:" + JsonHelper.EncodeJson(device));
                    //}
                }
                else
                {
                    TestLog4Net.LogHelper.Info("Error:激活码使用记录数据库失败。参数信息:" + JsonHelper.EncodeJson(useinfo));
                }
            }
            else
            {
                int i = Add<tb_batchactivateusedevice>(device);
                if (i > 0)
                {
                    TestLog4Net.LogHelper.Info("Error:激活码使用记录数据库失败。参数信息:" + JsonHelper.EncodeJson(device));
                }
            }
            TestLog4Net.LogHelper.Info(useinfo.activatecode + "-Out Rang End");
        }







    }
}
