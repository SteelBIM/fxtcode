using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using log4net;
using FxtSpider.FxtApi.ApiManager;

namespace FxtSpider.Bll.FxtApiManager
{
    public static class CaseApiManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CaseApiManager));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="failList">返回失败的案例</param>
        /// <returns></returns>
        public static bool ImportCase(List<VIEW_案例信息_城市表_网站表> list, out List<VIEW_案例信息_城市表_网站表> failList,out string message)
        {
            message = "";
            failList = new List<VIEW_案例信息_城市表_网站表>();
            List<案例库上传信息过滤表> 过滤案例List = null;
            Dictionary<long, int> dic = new Dictionary<long, int>();
            if (!CaseApi.发布需要整理的数据到服务器(list, out 过滤案例List,out dic))
            {
                message = "fxt服务异常";
                log.Debug(string.Format("发布需要整理的数据到服务器_异常:(案例ID个数:{0})", list.Count));
                return false;
            }
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                db.Connection.Open();
                //开始事务
                var tran = db.Connection.BeginTransaction();
                try
                {
                    db.Transaction = tran;

                    //删除此次案例的滤记录
                    List<long> caseIdList = new List<long>();
                    if (list != null)
                    {
                        foreach (VIEW_案例信息_城市表_网站表 caseView in list)
                        {
                            caseIdList.Add(caseView.ID);
                        }
                        if (!CaseFilterLogManager.DeleteCaseFilterByCaseIds(caseIdList.ToArray(), db: db))
                        {
                            message = "系统异常";
                            log.Debug(string.Format("删除此次案例的滤记录_异常:(案例ID个数:{0},过滤ID数组个数:{1})",
                                 list.Count, 过滤案例List.Count));
                            tran.Rollback();
                            return false;
                        }
                    }

                    //记录过滤掉的案例ID
                    if (过滤案例List != null && 过滤案例List.Count > 0)
                    {
                        foreach (案例库上传信息过滤表 obj in 过滤案例List)
                        {
                            VIEW_案例信息_城市表_网站表 obj2 = list.Find(delegate(VIEW_案例信息_城市表_网站表 obj3) { return obj3.ID == obj.案例ID; });
                            if (obj2 != null)
                            {
                                failList.Add(obj2);
                            }
                        }
                        if (!CaseFilterLogManager.将上传时被过滤的信息记录到表中(过滤案例List, db))
                        {
                            message = "将上传时被过滤的信息记录到表中_系统异常";
                            log.Debug(string.Format("将上传时被过滤的信息记录到表中_异常:(案例ID个数:{0},过滤ID数组个数:{1})",
                                 list.Count, 过滤案例List.Count));
                            tran.Rollback();
                            return false;
                        }
                    }
                    //统计入库失败的案例个数
                    if (!ProjectCaseCountManager.UpdateNotImportCaseCount(list, 过滤案例List, out message, new DAL.DB.DataClass(db)))
                    {
                        log.Debug(string.Format("统计入库失败的案例个数_系统异常:(案例ID个数{0})", list.Count));
                        tran.Rollback();
                        return false;
                    }
                    //记录上传成功的案例ID
                    if (!CaseLogManager.将当前已经整理入库的案例记录表中(list,dic, db))
                    {
                        message = "将当前已经整理入库的案例记录表中_系统异常";
                        log.Debug(string.Format("将当前已经整理入库的案例记录表中_系统异常:(案例ID个数{0})", list.Count));
                        tran.Rollback();
                        return false;
                    }

                    db.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    message = "系统异常";
                    log.Error(string.Format("系统异常ImportCase(List<VIEW_案例信息_城市表_网站表> list, out List<VIEW_案例信息_城市表_网站表> failList)", list.Count), ex);
                    tran.Rollback();
                    return false;
                }
            }

            return true;
        }

    }
}
