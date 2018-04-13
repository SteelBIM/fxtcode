using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseActivate.Core.Utility;
using CourseActivate.Activate.Constract.Model;
using System.Data;
using CourseActivate.Framework.BLL;
using System.Transactions;
using System.Data.SqlClient;
using CourseActivate.Framework.DAL;

namespace CourseActivate.Activate.BLL
{

    /// <summary>
    /// 激活课程业务代码
    /// </summary>
    public class ActivateCourseBLL : Manage
    {
        // Framework.BLL.Manage manager = new Framework.BLL.Manage();
        Activate.DAL.ActivateCourseDAL manager = new DAL.ActivateCourseDAL();

        /// <summary>
        /// 激活码记录
        /// </summary>
        tb_batchactivate bainfo = new tb_batchactivate();

        /// <summary>
        /// 批次信息
        /// </summary>
        tb_batch binfo = new tb_batch();

        /// <summary>
        /// 激活码类型数据
        /// </summary>
        tb_activatetype atinfo = new tb_activatetype();

        /// <summary>
        /// 激活码使用记录
        /// </summary>
        tb_batchactivateuse bauinfo = new tb_batchactivateuse();

        /// <summary>
        /// 激活码使用设备记录
        /// </summary>
        List<tb_batchactivateusedevice> baudlist = new List<tb_batchactivateusedevice>();

        /// <summary>
        /// 课程编号
        /// </summary>
        int? BookID = null;
        /// <summary>
        /// 激活码已激活的设备数
        /// </summary>
        int DeviceCount = 1;
        /// <summary>
        /// 激活码可以激活的所有设备数
        /// </summary>
        int TotalDeviceCount = 0;
        /// <summary>
        /// 设备激活时间
        /// </summary>
        DateTime DeviceDateTime = DateTime.Now;

        /// <summary>
        /// 批次课程信息
        /// </summary>
        tb_batchbooks bbinfo = new tb_batchbooks();

        public string GetActivateEndTime(string userId, string bookId)
        {

            string sql = string.Format(@" SELECT TOP 1 createtime FROM tb_batchactivateuse WHERE userid={0} AND bookid={1} ORDER BY createtime ASC", userId, bookId);
            string time = manager.SelectString(sql);
            return time;
        }

        /// <summary>
        /// 激活课程
        /// </summary>
        /// <param name="activateCode"></param>
        /// <param name="deviceType"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public KingResponse ActivateCourse(string activateCode, int deviceType, string deviceCode, int isios = 0)
        {
            ///返回字符串的结果，使用‘|’分开激活码使用信息和bookID
            string resultStr = ",\"DeviceCount\":{0},\"TotalDeviceCount\":{1},\"Months\":1200,\"DeviceActivateDate\":\"{2}\"|{3}";

            //初始化数据，从缓存中读取
            int checkresult = InitData(activateCode);
            if (checkresult != 0)
            {
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }
            //激活码没有使用过
            if (bauinfo == null)
            {
                //判断激活码是否有效
                checkresult = CheckActivateCode(activateCode);
                if (checkresult != 0)
                {
                    return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
                }
            }

            //判断激活码使用的范围
            checkresult = CheckActivateCodeSpace();
            if (checkresult != 0)
            {
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }
            if (!BookID.HasValue || BookID.Value == 0)
            {
                return KingResponse.GetErrorResponse(107, dic[107]);
            }

            //判断激活码使用的次数
            checkresult = CheckActivateCodeDevice(deviceType, deviceCode);
            if (checkresult != 0)
            {
                //当前设备已经使用过此激活码
                if (checkresult == 113)
                {
                    resultStr = string.Format(resultStr, DeviceCount, TotalDeviceCount, DeviceDateTime.ToString("yyyy-MM-dd HH:mm:ss"), BookID);
                    return KingResponse.GetResponse(resultStr);
                }
                if (checkresult == 119)
                {
                    return KingResponse.GetErrorResponse(checkresult, "音频设备使用数量已达到" + atinfo.devicenum + "台");
                }
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }

            //修改数据库数据，保留激活记录
            checkresult = SaveProcess(BookID.Value, deviceCode, deviceType, isios);
            if (checkresult != 0)
            {
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }
            resultStr = string.Format(resultStr, DeviceCount, TotalDeviceCount, DeviceDateTime.ToString("yyyy-MM-dd HH:mm:ss"), BookID);
            return KingResponse.GetResponse(resultStr);
        }

        /// <summary>
        /// 通过激活码获取书本编号
        /// </summary>
        /// <param name="activateCode"></param>
        /// <returns></returns>
        public KingResponse GetBookID(string activateCode)
        {
            string resultStr = "{0}|,\"Remark\":\"{1}\""; //BookID=1 | Remark=该激活码还能在*台设备上使用
            //初始化数据，从缓存中读取
            int checkresult = InitData(activateCode);
            if (checkresult != 0)
            {
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }
            //激活码没有使用过
            if (bauinfo == null)
            {
                //判断激活码是否有效
                checkresult = CheckActivateCode(activateCode);
                if (checkresult != 0)
                {
                    return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
                }
            }

            //判断激活码使用的范围
            checkresult = CheckActivateCodeSpace();
            if (checkresult != 0)
            {
                return KingResponse.GetErrorResponse(checkresult, dic[checkresult]);
            }
            if (!BookID.HasValue || BookID.Value == 0)
            {
                return KingResponse.GetErrorResponse(107, dic[107]);
            }
            string Remark = "该激活码仅能在" + TotalDeviceCount.ToString() + "台设备上使用";
            resultStr = string.Format(resultStr, BookID, Remark);
            return KingResponse.GetResponse(resultStr);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="activateCode"></param>
        /// <returns></returns>
        private int InitData(string activateCode)
        {

            //using (var redisClient = RedisRepository.GetClient())
            //{
            //    bainfo = redisClient.Get<tb_batchactivate>(RedisConfiguration.GetActivateKey(activateCode));
            //    if (bainfo == null)
            //    {
            //        //激活码不存在
            //        return 101;
            //    }
            //    binfo = redisClient.Get<tb_batch>(RedisConfiguration.GetBatchKey(bainfo.batchid.Value));
            //    atinfo = redisClient.Get<tb_activatetype>(RedisConfiguration.GetActivateTypeKey(binfo.activatetypeid.Value));
            //    bauinfo = redisClient.Get<tb_batchactivateuse>(RedisConfiguration.GetActivateUseKey(bainfo.activateid));
            //    bbinfo = redisClient.Get<tb_batchbooks>(RedisConfiguration.GetBatchBookKey(bainfo.batchid.Value));
            //    if (bauinfo != null)
            //    {
            //        baudlist = redisClient.Get<List<tb_batchactivateusedevice>>(RedisConfiguration.GetActivateUseDeviceKey(bauinfo.activateuseid.Value));
            //    }
            //    if (atinfo == null)
            //    {
            //        //未知的激活码类型
            //        return 201;
            //    }
            //    //设置激活码可以激活设备数量
            //    TotalDeviceCount = atinfo.devicenum;
            //    if (bbinfo == null)
            //    {
            //        //批次课程信息不存在
            //        return 202;
            //    }
            //    return 0;
            //}
            bainfo = manager.GetBatchActivate(activateCode);
            if (bainfo == null)
            {
                //激活码不存在
                return 101;
            }
            binfo = manager.GetBatchInfo(bainfo.batchid.Value);
            atinfo = manager.GetActivateType(binfo.activatetypeid.Value);
            bauinfo = manager.GetActivateCodeUse(bainfo.activateid);
            bbinfo = manager.GetBatchBookInfo(binfo.batchid);
            if (bauinfo != null)
            {
                baudlist = manager.GetActivateCodeDeviceList(bauinfo.activateuseid.Value);
            }
            if (atinfo == null)
            {
                //未知的激活码类型
                return 201;
            }
            //设置激活码可以激活设备数量
            TotalDeviceCount = atinfo.devicenum;
            if (bbinfo == null)
            {
                //批次课程信息不存在
                return 202;
            }
            return 0;
        }

        /// <summary>
        /// 判断激活码是否有效
        /// </summary>
        /// <param name="activateCode"></param>
        /// <returns></returns>
        private int CheckActivateCode(string activateCode)
        {
            if (binfo.status == 0)//0-未启用，1-启用，2-禁用
            {
                //该批次激活码还未启用
                return 102;
            }
            if (binfo.status == 2)
            {
                //当前批次激活码已禁用
                return 117;
            }
            if (binfo.enddate < DateTime.Now)
            {
                //该批次激活码已过期
                return 103;
            }

            return 0;
        }

        /// <summary>
        ///  判断激活码使用的次数
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        private int CheckActivateCodeDevice(int deviceType, string deviceCode)
        {
            ///设备类型不等于1也不等于2 拒绝请求
            if (!(deviceType == 1 || deviceType == 2))
            {
                //当前设备类型只能是客户端或者移动设备
                return 116;
            }
            //激活码没有使用过
            //if (bauinfo == null)
            //{
            //    return 0;
            //}
            if (baudlist.Count > 0)
            {
                foreach (tb_batchactivateusedevice baud in baudlist)
                {
                    if (deviceCode == baud.devicecode)
                    {
                        DeviceCount = baudlist.Count;
                        DeviceDateTime = baud.createtime.Value;
                        //当前设备已经使用过此激活码
                        return 113;
                    }
                }
            }
            if (atinfo.status == 0)
            {
                //当前批次类型已禁用
                return 118;
            }

            int pccount = baudlist.Where(i => i.devicetype == 1).Count();
            int mobilecount = baudlist.Where(i => i.devicetype == 2).Count();
            if (atinfo.type.Value == 0)//使用终端类型 全部
            {
                int count = pccount + mobilecount;
                if (count >= atinfo.devicenum)
                {
                    //终端设备使用超过限制
                    return 119;
                }
            }
            else if (atinfo.type.Value == 1)//使用终端类型 PC
            {
                if (deviceType == 1)
                {
                    if (pccount >= atinfo.devicenum)
                    {
                        //pc端使用数量超过限制
                        return 110;
                    }
                }
                else
                {
                    //当前授权码不允许使用此类设备
                    return 112;
                }
            }
            else if (atinfo.type.Value == 2)//使用终端类型 移动
            {
                if (deviceType == 2)
                {
                    if (mobilecount >= atinfo.devicenum)
                    {
                        //移动端使用数量超过限制
                        return 111;
                    }
                }
                else
                {
                    //当前授权码不允许使用此类设备
                    return 112;
                }
            }
            DeviceCount = baudlist.Count != 0 ? baudlist.Count : 1;
            DeviceDateTime = DateTime.Now;
            return 0;
        }

        /// <summary>
        /// 判断激活码使用的范围（设备类型和课本编号）
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        private int CheckActivateCodeSpace()
        {
            //批次教程信息必须有值，没有返回错误
            //如果激活码被使用过了，使用激活过的教程

            if (!bbinfo.bookid.HasValue || bbinfo.bookid == 0)
            {
                ///当前激活码不能使用在此课本上
                ///新华的激活码必须有一个bookid
                return 104;
            }
            BookID = bbinfo.bookid;
            if (bauinfo != null)
            {
                //if (bauinfo.bookid != BookID)
                //{
                //    //当前激活课程与之前激活课程不匹配
                //    return 105;
                //}
                BookID = bauinfo.bookid;
            }
            return 0;
        }


        /// <summary>
        /// 修改数据库数据，保留激活记录
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="devicecode"></param>
        /// <param name="devicetype"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private int SaveProcess(int bookid, string devicecode, int devicetype, int isios)
        {
            if (bauinfo == null)
            {
                bauinfo = new tb_batchactivateuse();
                bauinfo.activateuseid = Guid.NewGuid();
                bauinfo.activatecode = bainfo.activatecode;
                bauinfo.activateid = bainfo.activateid;
                bauinfo.bookid = bookid;
                bauinfo.createtime = DateTime.Now;

                tb_batchactivateusedevice baud = new tb_batchactivateusedevice();
                baud.devicecode = devicecode;
                baud.devicetype = devicetype;
                baud.activateuseid = bauinfo.activateuseid;
                baud.isios = isios;
                baud.createtime = DateTime.Now;

                #region 保存到redis
                if (!manager.AddActivateUse(bauinfo))
                {
                    TestLog4Net.LogHelper.Info("Error:激活码使用记录redis失败。参数信息:" + JsonHelper.EncodeJson(bauinfo));
                }
                if (!manager.AddUseDevice(baud))
                {
                    TestLog4Net.LogHelper.Info("Error:激活码使用设备记录redis失败。参数信息:" + JsonHelper.EncodeJson(baud));
                }
                AddSyncRang(new SyncRangmodel { ActivateUse = bauinfo, ActivateUseDevice = baud });
                //批次激活码激活统计数量
                AddSyncBatch(binfo.batchid);
                #endregion
            }
            else
            {
                tb_batchactivateusedevice baud = new tb_batchactivateusedevice();
                baud.devicecode = devicecode;
                baud.devicetype = devicetype;
                baud.activateuseid = bauinfo.activateuseid;
                baud.isios = isios;
                baud.createtime = DateTime.Now;
                //算上本次激活设备数量
                DeviceCount++;
                #region 保存到redis
                if (!manager.AddUseDevice(baud))
                {
                    TestLog4Net.LogHelper.Info("Error:激活码使用设备记录redis失败。参数信息:" + JsonHelper.EncodeJson(baud));
                }
                AddSyncRang(new SyncRangmodel { ActivateUse = null, ActivateUseDevice = baud });
                #endregion
            }
            return 0;
        }

        /// <summary>
        /// 查找激活码
        /// </summary>
        /// <returns></returns>
        public string MatchActivateCode()
        {
            string code = "";
            string sql = string.Format(@"   declare @activatecode VARCHAR(50)
                                            SELECT TOP 1 @activatecode=c.activatecode
                                            FROM      tb_batch a
                                                    LEFT JOIN dbo.tb_activatetype b ON b.activatetypeid = a.activatetypeid
                                                    LEFT JOIN dbo.tb_batchactivate c ON c.batchid = a.batchid
                                            WHERE   a.status=1 and  b.type = 2 and  b.way = 1
                                                    AND c.ismatch = 0;
                                            update tb_batchactivate set ismatch=1 where activatecode=@activatecode
                                            SELECT @activatecode activatecode ");
            code = manager.SelectString(sql);
            return code;
        }

        /// <summary>
        /// 根据userId+bookId查找已使用的激活码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public List<tb_batchactivateuse> SearchBatchActivateUse(string userId, string bookId)
        {
            List<tb_batchactivateuse> ba = manager.SelectSearch<tb_batchactivateuse>(i => i.userid == userId && i.bookid == Convert.ToInt32(bookId));
            return ba;
        }

        public int UpdateActivateStauts(string activatecode)
        {
            string sql = string.Format(@"UPDATE tb_batchactivate SET ismatch=1 WHERE activatecode='{0}'", activatecode);
            return manager.ExecuteCommand(sql);
        }

        /// <summary>
        /// 设置激活码批次redis
        /// </summary>
        /// <param name="batchid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool SetBatchRedis(int batchid, int status)
        {
            return manager.SetBatch(batchid, status);
        }

        /// <summary>
        /// 设置激活码类型redis
        /// </summary>
        /// <param name="typeinfo"></param>
        /// <returns></returns>
        public bool SetActivateType(tb_activatetype typeinfo)
        {
            return manager.AddActivateType(typeinfo);
        }

        public bool RemoveActivateType(int typeid)
        {
            return manager.RemoveRedis(RedisConfiguration.ActivateTypeKey + typeid);
        }



        /// <summary>
        /// 激活码总使用数统计, tb_activatemonthrecord表
        /// </summary>
        /// <returns></returns>
        public bool ActivateMonthRecordStatistics()
        {
            //  var dailyActList = manager.GetRedis<List<tb_batchactivateuse>>(RedisConfiguration.ActivateStatiscKey + "Daily");//
            string key = "Daily" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd");//当日统计前一天的数据
            List<string> actList = manager.GetList(RedisConfiguration.GetActivateStatiscKey(key));
            if (actList == null || !actList.Any())
            {
                return true;//没有要统计的数据
            }
            var time = DateTime.Now.AddDays(-1);//本次统计的时间结点

            using (TransactionScope scope = new TransactionScope())
            {
                var records = SelectSearch<tb_activatemonthrecord>(o => o.month == time.Month && o.year == time.Year);//获取时间结点对应的月分记录
                var record = records.FirstOrDefault();
                bool success = false;
                if (record == null)
                {
                    record = new tb_activatemonthrecord { createtime = DateTime.Now, year = time.Year, month = time.Month, num = actList.Count };
                    success = manager.Add(record) > 0;
                }
                else
                {
                    record.num += actList.Count;
                    success = manager.Update(record);
                }

                if (success)
                {
                    //再处理课程激活码统计

                    //code****//
                    BookActivateRecordStatistics(actList, time);

                    //统计完成,清空redis记录
                    manager.RemoveRedis(RedisConfiguration.GetActivateStatiscKey(key));

                    scope.Complete();//提交事务

                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 课程激活码使用数统计(tb_res_bookactivaterecord表)
        /// </summary>
        /// <param name="currentDailyActList"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool BookActivateRecordStatistics(List<string> currentDailyActList, DateTime time)
        {
            var groups = currentDailyActList.GroupBy(o => o).ToList();//按课程分组
            groups.ForEach(o =>
            {
                var gBookid = int.Parse(o.Key);
                var record = SelectSearch<tb_res_bookactivaterecord>(b => b.bookid == gBookid).FirstOrDefault();
                if (record == null)
                {
                    record = new tb_res_bookactivaterecord { createtime = DateTime.Now, usenum = 0, bookid = int.Parse(o.Key) };
                }
                string sql = @"select COUNT(a.activatecode) from tb_batch b inner join tb_batchactivate a on b.batchid=a.batchid inner join tb_batchbooks bb on b.batchid=bb.batchid where bookid=@bookid";//该课程所有激活码总数
                var r = manager.SqlQuery<int>(sql, new List<SqlParameter>() { new SqlParameter { Value = o.Key, ParameterName = "bookid" } });
                record.num = r.FirstOrDefault();
                record.usenum += o.Count();//已激活的个数
                if (record.bookrecordid > 0)
                    manager.Update(record);
                else
                    manager.Add(record);
            });
            return true;
        }



        /// <summary>
        /// 批量移除redis缓存
        /// </summary>
        /// <param name="list"></param>
        public void Remove(List<string> list)
        {
            manager.RemoveRedis(list);
        }

        /// <summary>
        /// 移除redis缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            manager.RemoveRedis(key);
        }

        public void RemoveBatch(List<string> list, int batchid)
        {
            Remove(list);
            Remove(RedisConfiguration.GetBatchKey(batchid));
        }

        /// <summary>
        /// 错误代码字典
        /// </summary>
        public Dictionary<int, string> dic = ErrorMsgCode.ErrorDic;

        public bool AddSyncBatch(int batchid)
        {
            using (var client = RedisHelperRepository.GetClient())
            {
                client.PushItemToList(RedisConfiguration.GetSyncDBKey("BatchID"), batchid.ToString());
                return true;
            }
        }

        public bool AddSyncRang(SyncRangmodel model)
        {
            using (var client = RedisHelperRepository.GetClient())
            {
                client.PushItemToList(RedisConfiguration.GetSyncDBKey("ActivateUse"), JsonHelper.EncodeJson(model));
                return true;
            }
        }

        public bool SyncRedisToDB()
        {
            int count = ConfigItemHelper.SyncDBCount;
            using (var client = RedisHelperRepository.GetClient())
            {
                List<string> list = new List<string>();
                int l = client.GetListCount(RedisConfiguration.GetSyncDBKey("ActivateUse"));
                if (l > 0)
                {
                    if (l > count)
                    {
                        list = client.GetRangeFromList(RedisConfiguration.GetSyncDBKey("ActivateUse"), 0, count - 1);
                    }
                    else
                    {
                        list = client.GetAllItemsFromList(RedisConfiguration.GetSyncDBKey("ActivateUse"));
                    }
                    List<tb_batchactivateuse_copy> baulist = new List<tb_batchactivateuse_copy>();
                    List<tb_batchactivateusedevice_copy> baudlist = new List<tb_batchactivateusedevice_copy>();
                    List<tb_batchactivateusedevicenum> baudnlist = new List<tb_batchactivateusedevicenum>();
                    string updatecodedevice = "";
                    for (int i = 0; i < list.Count; i++)
                    {
                        SyncRangmodel sm = JsonHelper.JsonDeserialize<SyncRangmodel>(list[i]);
                        if (sm.ActivateUse != null)
                        {
                            tb_batchactivateuse_copy tmp = new tb_batchactivateuse_copy();
                            tmp.activatecode = sm.ActivateUse.activatecode;
                            tmp.activateid = sm.ActivateUse.activateid ?? 0;
                            tmp.activateuseid = sm.ActivateUse.activateuseid.Value;
                            tmp.bookid = sm.ActivateUse.bookid ?? 0;
                            tmp.createtime = sm.ActivateUse.createtime.Value.ToLocalTime();
                            baulist.Add(tmp);
                        }
                        if (sm.ActivateUseDevice != null)
                        {
                            if (sm.ActivateUse == null)
                            {
                                updatecodedevice += ",'" + sm.ActivateUseDevice.activateuseid + "'";
                            }
                            else
                            {
                                baudnlist.Add(new tb_batchactivateusedevicenum { activateuseid = sm.ActivateUseDevice.activateuseid, createTime = DateTime.Now, usenum = 1 });
                            }
                            tb_batchactivateusedevice_copy tmp = new tb_batchactivateusedevice_copy();
                            //tmp.activateusedeviceid = sm.ActivateUseDevice.activateusedeviceid??0;
                            tmp.activateuseid = sm.ActivateUseDevice.activateuseid.Value;
                            tmp.createtime = sm.ActivateUseDevice.createtime.Value.ToLocalTime();
                            tmp.devicecode = sm.ActivateUseDevice.devicecode;
                            tmp.devicetype = sm.ActivateUseDevice.devicetype ?? 0;
                            tmp.isios = sm.ActivateUseDevice.isios ?? 0;
                            baudlist.Add(tmp);
                        }
                    }
                    if (baudlist != null && baulist.Count > 0)
                    {
                        KingResponse res = MSSqlBulkCopy<tb_batchactivateuse_copy>(baulist, "tb_batchactivateuse");
                        if (res.Success)
                        {
                            res = MSSqlBulkCopy<tb_batchactivateusedevice_copy>(baudlist, "tb_batchactivateusedevice");
                            if (res.Success)
                            {
                                #region 删掉读取出来的数据
                                for (int i = 0; i < list.Count; i++)
                                {
                                    // client.
                                    string str = client.RemoveStartFromList(RedisConfiguration.GetSyncDBKey("ActivateUse"));
                                    if (string.IsNullOrEmpty(str))
                                    {
                                        TestLog4Net.LogHelper.Info("移除redis激活码失败:" + i.ToString());
                                    }
                                }
                                #endregion

                                #region 新增或者修改激活码使用设备数量
                                if (baudnlist != null && baudnlist.Count > 0)
                                {
                                    res = MSSqlBulkCopy<tb_batchactivateusedevicenum>(baudnlist, "tb_batchactivateusedevicenum");
                                    if (!res.Success)
                                    {
                                        TestLog4Net.LogHelper.Info("新增设备使用记录数失败。" + res.ErrorMsg);
                                    }
                                }
                                string sql = "update tb_batchactivateusedevicenum set usenum=usenum+1 where activateuseid in(" +
                                    (string.IsNullOrEmpty(updatecodedevice) ? "'" + Guid.NewGuid() + "'" : updatecodedevice.Substring(1)) + ")";
                                manager.ExecuteCommand(sql);
                                #endregion
                            }
                            else
                            {
                                TestLog4Net.LogHelper.Info("保存激活码使用设备失败。" + res.ErrorMsg);
                            }
                        }
                        else
                        {
                            TestLog4Net.LogHelper.Info("保存激活码使用记录失败。" + res.ErrorMsg);
                        }
                    }
                    else
                    {
                        KingResponse res = MSSqlBulkCopy<tb_batchactivateusedevice_copy>(baudlist, "tb_batchactivateusedevice");
                        if (res.Success)
                        {
                            #region 删掉读取出来的数据
                            for (int i = 0; i < list.Count; i++)
                            {
                                // client.
                                string str = client.RemoveStartFromList(RedisConfiguration.GetSyncDBKey("ActivateUse"));
                                if (string.IsNullOrEmpty(str))
                                {
                                    TestLog4Net.LogHelper.Info("移除redis激活码失败:" + i.ToString());
                                }
                            }
                            #endregion

                            #region 新增或者修改激活码使用设备数量
                            if (baudnlist != null && baudnlist.Count > 0)
                            {
                                res = MSSqlBulkCopy<tb_batchactivateusedevicenum>(baudnlist, "tb_batchactivateusedevicenum");
                                if (!res.Success)
                                {
                                    TestLog4Net.LogHelper.Info("新增设备使用记录数失败。" + res.ErrorMsg);
                                }
                            }
                            string sql = "update tb_batchactivateusedevicenum set usenum=usenum+1 where activateuseid in(" +
                                (string.IsNullOrEmpty(updatecodedevice) ? "'" + Guid.NewGuid() + "'" : updatecodedevice.Substring(1)) + ")";
                            manager.ExecuteCommand(sql);
                            #endregion
                        }
                        else
                        {
                            TestLog4Net.LogHelper.Info("保存激活码使用设备失败。" + res.ErrorMsg);
                        }
                    }
                }
                #region 激活码批次使用次数统计
                l = client.GetListCount(RedisConfiguration.GetSyncDBKey("BatchID"));
                if (l > 0)
                {
                    if (l > count)
                    {
                        list = client.GetRangeFromList(RedisConfiguration.GetSyncDBKey("BatchID"), 0, count - 1);
                    }
                    else
                    {
                        list = client.GetAllItemsFromList(RedisConfiguration.GetSyncDBKey("BatchID"));
                    }
                    var q = from s in list
                            group s by s into g
                            select new { batchid = g.Key, count = g.Count() };
                    string sql = "";
                    foreach (var v in q)
                    {
                        sql += "update tb_batch set activateusenum=activateusenum+" + v.count + " where batchid=" + v.batchid + ";";
                    }
                    TestLog4Net.LogHelper.Info("更新批次激活码使用数量。" + sql);
                    manager.ExecuteCommand(sql);
                    for (int i = 0; i < list.Count; i++)
                    {
                        // client.
                        string str = client.RemoveStartFromList(RedisConfiguration.GetSyncDBKey("BatchID"));
                        if (string.IsNullOrEmpty(str))
                        {
                            TestLog4Net.LogHelper.Info("移除redis批次使用数量失败:" + i.ToString());
                        }
                    }
                }
                #endregion
            }
            return true;
        }

        public void AddResitTestData(int count)
        {
            using (var client = RedisHelperRepository.GetClient())
            {
                client.Remove(RedisConfiguration.GetSyncDBKey("ActivateUse"));
                for (int i = 0; i < count; i++)
                {
                    SyncRangmodel model = new SyncRangmodel();
                    Guid g = Guid.NewGuid();
                    model.ActivateUse = new tb_batchactivateuse { activatecode = "XDSTUR8BW", activateid = 10303545 + i, bookid = 24, createtime = DateTime.Now, activateuseid = g };
                    model.ActivateUseDevice = new tb_batchactivateusedevice { activateuseid = g, createtime = DateTime.Now, devicecode = "12321321321312" };
                    client.PushItemToList(RedisConfiguration.GetSyncDBKey("ActivateUse"), JsonHelper.EncodeJson(model));
                }
                client.Remove(RedisConfiguration.GetSyncDBKey("BatchID"));
                for (int i = 0; i < count; i++)
                {
                    int k = i % 2;
                    client.PushItemToList(RedisConfiguration.GetSyncDBKey("BatchID"), "100" + k.ToString());
                }
            }
        }

        public bool RemoveRedisList()
        {
            int count = 5000;
            using (var client = RedisHelperRepository.GetClient())
            {
                List<string> list = new List<string>();
                int l = client.GetListCount(RedisConfiguration.GetSyncDBKey("Test"));
                if (l > count)
                {
                    list = client.GetRangeFromList(RedisConfiguration.GetSyncDBKey("Test"), 0, count - 1);
                }
                else
                {
                    list = client.GetAllItemsFromList(RedisConfiguration.GetSyncDBKey("Test"));
                }
                for (int i = 0; i < list.Count; i++)
                {
                    string str = client.RemoveStartFromList(RedisConfiguration.GetSyncDBKey("Test"));
                    if (!string.IsNullOrEmpty(str))
                    {
                        TestLog4Net.LogHelper.Error("移除redis激活码失败:" + i.ToString());
                    }
                }
            }
            return true;
        }

    }



}
