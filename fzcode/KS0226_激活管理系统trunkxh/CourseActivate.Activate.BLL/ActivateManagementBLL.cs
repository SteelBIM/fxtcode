using CourseActivate.Activate.Constract.Model;
using CourseActivate.Activate.DAL;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Diagnostics;

namespace CourseActivate.Activate.BLL
{
    public class ActivateManagementBLL : Manage
    {
        ActivateManagementDAL activateManagementDal = new ActivateManagementDAL();

        /// <summary>
        /// 导入生成的激活码
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="activateCodeList"></param>
        /// <returns></returns>
        public KingResponse InsertBatchActivateCode(int actNum, int typeId, int bookId, string remark)
        {
            KingResponse response = new KingResponse();
            //以下三个参数之所以不能放在GetRandomChars()方法中,是因为每次在方法里面new会造成性能损耗(比如生成1000000个激活码,差距很明显.)
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            string chars = "ABCDEFHJKLMNPQRSTUVWXY23456789";
            int batchKey;//用于后面redis

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    #region try
                    string batchCode = GetRandomChars(3, rand, sb, chars);
                    var batchInDB = SelectSearch<tb_batch>(o => o.batchcode == batchCode);
                    while (batchInDB != null && batchInDB.Any())//批次已存在
                    {
                        batchCode = GetRandomChars(3, rand, sb, chars);//重新生成批次
                        batchInDB = SelectSearch<tb_batch>(o => o.batchcode == batchCode);
                    }
                    tb_batch batch = new tb_batch { activatenum = actNum, batchcode = batchCode, status = 0, remark = remark, activatetypeid = typeId, createtime = DateTime.Now, startdate = DateTime.Now, enddate = DateTime.Now.AddYears(1), purpose = 0, indate = 12, createtype = "系统生成" };

                    int batchIdFromDB = Add(batch);
                    batchKey = batchIdFromDB;
                    if (batchIdFromDB <= 0)
                    {
                        throw new Exception("批次插入失败");
                    }
                    List<tb_batchactivate_copy> actList = new List<tb_batchactivate_copy>();
                    string[] actArray = new string[] { };
                    Stopwatch watch = new Stopwatch();
                    watch.Start();

                    string tempCode;
                    for (int i = 0; i < actNum; i++)
                    {//第一遍生成,先不判断重复(因为如果是百万级别,会导致很慢甚至卡死)
                        tempCode = batchCode + GetRandomChars(6, rand, sb, chars);
                        //while (actList.Any(o => o.activatecode == tempCode))//
                        //{
                        //    tempCode = batchCode + GetRandomChars(6, rand,sb,chars);//保证不重复
                        //}

                        actList.Add(new tb_batchactivate_copy
                        {
                            batchid = batchIdFromDB,
                            activatecode = tempCode,
                            createtime = DateTime.Now
                        });
                    }
                    string[] distinctCodes = actList.Select(o => o.activatecode).Distinct().ToArray();
                    for (int i = 0; i < actNum - distinctCodes.Count(); i++)
                    {
                        tempCode = batchCode + GetRandomChars(6, rand, sb, chars);
                        //while (actList.Any(o => o.activatecode == tempCode))//判断重复
                        //{
                        //    tempCode = batchCode + GetRandomChars(6, rand, sb, chars);//保证不重复
                        //}

                        actList.Add(new tb_batchactivate_copy
                        {
                            batchid = batchIdFromDB,
                            activatecode = tempCode,
                            createtime = DateTime.Now
                        });
                    }
                    distinctCodes = actList.Select(o => o.activatecode).Distinct().ToArray();
                    for (int i = 0; i < actNum - distinctCodes.Count(); i++)
                    {
                        tempCode = batchCode + GetRandomChars(6, rand, sb, chars);
                        while (actList.Any(o => o.activatecode == tempCode))//判断重复
                        {
                            tempCode = batchCode + GetRandomChars(6, rand, sb, chars);//保证不重复
                        }
                        actList.Add(new tb_batchactivate_copy
                        {
                            batchid = batchIdFromDB,
                            activatecode = tempCode,
                            createtime = DateTime.Now
                        });
                    }
                    watch.Stop();
                    distinctCodes = actList.Select(o => o.activatecode).Distinct().ToArray();
                    actList.Clear();
                    foreach (string c in distinctCodes)
                    {
                        actList.Add(new tb_batchactivate_copy
                        {
                            batchid = batchIdFromDB,
                            activatecode = c,
                            createtime = DateTime.Now
                        });
                    }
                    Stopwatch watch2 = new Stopwatch();
                    watch2.Start();
                    var success = MSSqlBulkCopy<tb_batchactivate_copy>(actList, "tb_batchactivate");//主键list
                    watch2.Stop();
                    if (success.Success)//全部成功插入
                    {
                        if (bookId > -1)//插入绑定的课程
                        {
                            tb_batchbooks bb = new tb_batchbooks() { batchid = batchIdFromDB, bookid = bookId, createTime = DateTime.Now };
                            var bbKey = Add(bb);//插入tb_batchbooks表
                            if (bbKey > 0)
                            {
                                response.Success = true;
                                response.Data = "导入成功,本次批次号为:" + batchCode;
                                scope.Complete();
                            }
                        }
                        else
                        {
                            response.Success = true;
                            response.Data = "导入成功,本次批次号为:" + batchCode;
                            scope.Complete();
                        }

                    }
                    #endregion
                }
                new Framework.DAL.DoRedisHash().SetEntryInHash("AddActivateCodeRange", batchKey.ToString(), "导入redis激活码进入队列:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Task.Run(() => new ActivateCourseDAL().InitBatchRedis(batchKey));
            }
            catch (Exception ex)
            {
                response.ErrorMsg = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 判断当前批次是否在导入到redis过程中
        /// </summary>
        /// <param name="batchid"></param>
        /// <returns></returns>
        public bool CheckBatchIsInsertRedis(int batchid)
        {
            string str = new Framework.DAL.DoRedisHash().GetValueFromHash("AddActivateCodeRange", batchid.ToString());
            if (string.IsNullOrEmpty(str)) {
                return false;
            }
            return true;
        }

        string GetRandomChars(int length, Random rand, StringBuilder sb, string chars)
        {
            sb.Clear();
            //rand = new Random();
            //StringBuilder sb = new StringBuilder(length);
            do
            {
                int randNum = rand.Next(30);
                sb.Append(chars[randNum]);
            }
            while (sb.Length < length);

            return sb.ToString();
        }//生成随机字母和数字
    }
}
