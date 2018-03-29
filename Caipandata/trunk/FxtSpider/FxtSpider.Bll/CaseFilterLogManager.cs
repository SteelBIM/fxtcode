using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using log4net;
using FxtSpider.Common;

namespace FxtSpider.Bll
{
    /// <summary>
    /// 案例上传时的过滤记录
    /// </summary>
    public static class CaseFilterLogManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CaseFilterLogManager));
        public static bool 将上传时被过滤的信息记录到表中(int 城市ID, int 网站ID, List<案例库上传信息过滤表> 过滤案例List, DataClassesDataContext db = null)
        {
            if (城市ID < 1 || 网站ID < 1 || 过滤案例List == null || 过滤案例List.Count < 1)
            {
                return true;
            }
            List<案例库上传信息过滤表> list = new List<案例库上传信息过滤表>();
            foreach (案例库上传信息过滤表 obj in 过滤案例List)
            {
                案例库上传信息过滤表 caseObj = new 案例库上传信息过滤表 { 案例ID = obj.案例ID, 城市ID = 城市ID, 网站ID = 网站ID, 过滤时间 = DateTime.Now, 错误说明 = obj.错误说明 };
                list.Add(caseObj);
            }
            bool exstisDb = true;
            if (db == null)
            {
                exstisDb = false;
                db = new DataClassesDataContext();
            }
            try
            {
                db.案例库上传信息过滤表.InsertAllOnSubmit<案例库上传信息过滤表>(list);
                db.SubmitChanges();
                if (!exstisDb)
                {
                    db.Connection.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("将上传时被过滤的信息记录到表中_异常,(城市ID:{0},网站ID{1},当前案例ID个数:{2})",
                    城市ID, 网站ID, 过滤案例List.Count),
                    ex);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
                return false;
            }
            return true;
        }
        public static bool 将上传时被过滤的信息记录到表中(List<案例库上传信息过滤表> 过滤案例List, DataClassesDataContext db = null)
        {
            if (过滤案例List == null || 过滤案例List.Count < 1)
            {
                return true;
            }
            bool exstisDb = true;
            if (db == null)
            {
                exstisDb = false;
                db = new DataClassesDataContext();
            }
            try
            {
                db.案例库上传信息过滤表.InsertAllOnSubmit<案例库上传信息过滤表>(过滤案例List);
                db.SubmitChanges();             
                if (!exstisDb)
                {
                    db.Connection.Close();
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("将上传时被过滤的信息记录到表中_异常,(当前案例ID个数:{0})", 过滤案例List.Count),
                    ex);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
                return false;
            }
            return true;
        }
        public static bool 将上传时被过滤的信息记录到表中(int 网站ID, List<案例库上传信息过滤表> 过滤案例List, DataClassesDataContext db = null)
        {
            if (网站ID < 1 || 过滤案例List == null || 过滤案例List.Count < 1)
            {
                return true;
            }
            List<案例库上传信息过滤表> list = new List<案例库上传信息过滤表>();
            bool exstisDb = true;
            if (db == null)
            {
                exstisDb = false;
                db = new DataClassesDataContext();
            }
            try
            {
                db.案例库上传信息过滤表.InsertAllOnSubmit<案例库上传信息过滤表>(过滤案例List);
                db.SubmitChanges();
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("将上传时被过滤的信息记录到表中_异常,(城市ID:所有,网站ID{0},当前案例ID个数:{1})",
                    网站ID, 过滤案例List.Count),
                    ex);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
                return false;
            }
            return true;
        }

        public static bool DeleteCaseFilterByCaseIds(long[] caseIds, DataClassesDataContext db = null)
        {
            string caseIdStr = caseIds.ConvertToString();
            if (string.IsNullOrEmpty(caseIdStr))
            {
                return true;
            }
            bool exstisDb = true;
            if (db == null)
            {
                exstisDb = false;
                db = new DataClassesDataContext();
            }
            try
            {
                db.案例库上传信息过滤表_DeleteByCaseIds(caseIdStr);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("将上次成功的信息从过滤表中删除_异常",
                    ex);
                if (!exstisDb)
                {
                    db.Connection.Close();
                }
                return false;
            }
            return true;
        }
    }
}
