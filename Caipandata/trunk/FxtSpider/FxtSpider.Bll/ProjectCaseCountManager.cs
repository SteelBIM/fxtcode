using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;

namespace FxtSpider.Bll
{
    public static class ProjectCaseCountManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ProjectCaseCountManager));

        public static SysData_ProjectCaseCount GetProjectCaseCountByProjectAndAreaId(long projectId,long areaId,DataClass _db = null)
        {

            DataClass db = new DataClass(_db);
            SysData_ProjectCaseCount obj = null;
            obj = db.DB.SysData_ProjectCaseCount.FirstOrDefault(p => p.ProjectId == projectId && p.AreaId == areaId);
            db.Connection_Close();
            db.Dispose();
            return obj;
        }
        public static SysData_ProjectCaseCount InsertProjectCaseCount(long projectId, long areaId,int notImportCaseCount, DataClass _db = null)
        {

            DataClass db = new DataClass(_db);
            SysData_ProjectCaseCount obj = null;
            obj = GetProjectCaseCountByProjectAndAreaId(projectId, areaId, db);
            if (obj == null)
            {
                obj = new SysData_ProjectCaseCount();
                obj.ProjectId = projectId;
                obj.AreaId = areaId;
                obj.NotImportCaseCount = notImportCaseCount;
                db.DB.SysData_ProjectCaseCount.InsertOnSubmit(obj);
                db.DB.SubmitChanges();
            }
            db.Connection_Close();
            db.Dispose();
            return obj;
        }
        public static SysData_ProjectCaseCount GetById(long id, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_ProjectCaseCount obj = null;
            obj = db.DB.SysData_ProjectCaseCount.FirstOrDefault(p => p.ID == id);
            db.Connection_Close();
            db.Dispose();
            return obj;
        }
        public static bool UpdateNotImportCaseCount(long projectId, long areaId,int count,  DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_ProjectCaseCount obj = GetProjectCaseCountByProjectAndAreaId(projectId, areaId, db);
            bool result = false;
            if (obj != null)
            {
                obj.NotImportCaseCount = obj.NotImportCaseCount + count;
                db.DB.SubmitChanges();
                result = true;
            }
            else
            {
                InsertProjectCaseCount(projectId, areaId, count, db);
                result = true;
            }
            db.Connection_Close();
            db.Dispose();
            return result;
        }
        /// <summary>
        /// 根据传过来的案例list获取期对应的SysData_ProjectCaseCount信息
        /// </summary>
        /// <param name="caseList">案例list</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        static List<SysData_ProjectCaseCount> GetSysData_ProjectCaseCountByCase(List<VIEW_案例信息_城市表_网站表> caseList, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            List<long> projectIds = new List<long>();
            List<long> areaIds = new List<long>();
            foreach (VIEW_案例信息_城市表_网站表 obj in caseList)
            {
                if (projectIds.Where(p => p == Convert.ToInt64(obj.ProjectId)).FirstOrDefault() == 0)
                {
                    projectIds.Add(Convert.ToInt64(obj.ProjectId));
                }
                if (areaIds.Where(p => p == Convert.ToInt64(obj.AreaId)).FirstOrDefault() == 0)
                {
                    areaIds.Add(Convert.ToInt64(obj.AreaId));
                }
            }
            List<SysData_ProjectCaseCount> list = db.DB.SysData_ProjectCaseCount.Where(tbl => projectIds.Contains(tbl.ProjectId) && areaIds.Contains(tbl.AreaId)).ToList();
            db.Connection_Close();
            db.Dispose();
            return list;
        }
        /// <summary>
        /// 根据传过来的案例list获取期对应的SysData_ProjectCaseCount信息
        /// </summary>
        /// <param name="caseList">案例list</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        static List<SysData_ProjectCaseCount> GetSysData_ProjectCaseCountByCase(List<案例信息> caseList, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            List<long> projectIds = new List<long>();
            List<long> areaIds = new List<long>();
            foreach (案例信息 obj in caseList)
            {
                if (projectIds.Where(p => p == Convert.ToInt64(obj.ProjectId)).FirstOrDefault() == null)
                {
                    projectIds.Add(Convert.ToInt64(obj.ProjectId));
                }
                if (areaIds.Where(p => p == Convert.ToInt64(obj.AreaId)).FirstOrDefault() == null)
                {
                    areaIds.Add(Convert.ToInt64(obj.AreaId));
                }
            }
            List<SysData_ProjectCaseCount> list = db.DB.SysData_ProjectCaseCount.Where(tbl => projectIds.Contains(tbl.ProjectId) && areaIds.Contains(tbl.AreaId)).ToList();
            db.Connection_Close();
            db.Dispose();
            return list;
        }
        /// <summary>
        /// 导入案例后统计未入库案例个数
        /// </summary>
        /// <param name="importCaseList">准备导入的案例</param>
        /// <param name="notImportCase">导入失败的案例</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool UpdateNotImportCaseCount(List<VIEW_案例信息_城市表_网站表> importCaseList, List<案例库上传信息过滤表> notImportCase,out string message, DataClass _db = null)
        {
            message = "";
            DataClass db = new DataClass(_db);
            try
            {
                //设置案例楼盘ID,行政区ID,片区ID
                importCaseList=ProjectManager.SetProjectId(importCaseList, _dc: db);
                List<SysData_ProjectCaseCount> addCount = new List<SysData_ProjectCaseCount>();
                List<SysData_ProjectCaseCount> updateCount = GetSysData_ProjectCaseCountByCase(importCaseList, _db: db);
                foreach (VIEW_案例信息_城市表_网站表 caseObj in importCaseList)
                {
                    if (caseObj.fxtId != null)
                    {
                        continue;
                    }
                    bool nowObj = false;//true:新实体,false:存在实体
                    SysData_ProjectCaseCount countObj = updateCount.Where(tbl => tbl.ProjectId == Convert.ToInt64(caseObj.ProjectId) && tbl.AreaId == Convert.ToInt64(caseObj.AreaId)).FirstOrDefault();
                    //个数记录不存在
                    if (countObj == null)
                    {
                        countObj = addCount.Where(tbl => tbl.ProjectId == Convert.ToInt64(caseObj.ProjectId) && tbl.AreaId == Convert.ToInt64(caseObj.AreaId)).FirstOrDefault();
                        if (countObj == null)
                        {
                            nowObj = true;
                            countObj = new SysData_ProjectCaseCount { ProjectId = Convert.ToInt64(caseObj.ProjectId), AreaId = Convert.ToInt64(caseObj.AreaId), NotImportCaseCount = 0 };
                        }
                    }
                    案例库上传信息过滤表 notImport = notImportCase.Where(tbl => tbl.案例ID == caseObj.ID).FirstOrDefault();
                    //当前为导入成功的
                    if (notImport == null)
                    {
                        //当前案例为曾经导入失败的案例
                        if (caseObj.是否已进行入库整理 == 1 && countObj.NotImportCaseCount > 0)
                        {
                            countObj.NotImportCaseCount = countObj.NotImportCaseCount - 1;
                        }
                    }
                    else//当前为导入失败的
                    {
                        //当前案例为曾经导入失败的案例
                        if (caseObj.是否已进行入库整理 != 1)
                        {
                            countObj.NotImportCaseCount = countObj.NotImportCaseCount + 1;
                        }
                    }
                    if (nowObj)
                    {
                        addCount.Add(countObj);
                    }
                }
                db.DB.SysData_ProjectCaseCount.InsertAllOnSubmit(addCount);
                db.DB.SubmitChanges();
            }
            catch (Exception ex)
            {
                db.Connection_Close();
                db.Dispose();
                message = "导入案例后统计未入库案例个数_系统异常";
                log.Error("UpdateNotImportCaseCount(List<VIEW_案例信息_城市表_网站表> importCaseList, List<案例库上传信息过滤表> notImportCase,out string message, DataClass _db = null)", ex);
         
                return false;
            }
            db.Connection_Close();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 导入案例后统计未入库案例个数
        /// </summary>
        /// <param name="importCaseList">准备导入的案例</param>
        /// <param name="notImportCase">导入失败的案例</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool UpdateNotImportCaseCount(List<案例信息> importCaseList, List<案例库上传信息过滤表> notImportCase, out string message, DataClass _db = null)
        {
            message = "";
            DataClass db = new DataClass(_db);
            try
            {
                //设置案例楼盘ID,行政区ID,片区ID
                importCaseList = ProjectManager.SetProjectId(importCaseList, _dc: db);
                List<SysData_ProjectCaseCount> addCount = new List<SysData_ProjectCaseCount>();
                List<SysData_ProjectCaseCount> updateCount = GetSysData_ProjectCaseCountByCase(importCaseList, _db: db);
                foreach (案例信息 caseObj in importCaseList)
                {
                    if (caseObj.fxtId != null)
                    {
                        continue;
                    }
                    bool nowObj = false;//true:新实体,false:存在实体
                    SysData_ProjectCaseCount countObj = updateCount.Where(tbl => tbl.ProjectId == Convert.ToInt64(caseObj.ProjectId) && tbl.AreaId == Convert.ToInt64(caseObj.AreaId)).FirstOrDefault();
                    //个数记录不存在
                    if (countObj == null)
                    {
                        countObj = addCount.Where(tbl => tbl.ProjectId == Convert.ToInt64(caseObj.ProjectId) && tbl.AreaId == Convert.ToInt64(caseObj.AreaId)).FirstOrDefault();
                        if (countObj == null)
                        {
                            nowObj = true;
                            countObj = new SysData_ProjectCaseCount { ProjectId = Convert.ToInt64(caseObj.ProjectId), AreaId = Convert.ToInt64(caseObj.AreaId), NotImportCaseCount = 0 };
                        }
                    }
                    案例库上传信息过滤表 notImport = notImportCase.Where(tbl => tbl.案例ID == caseObj.ID).FirstOrDefault();
                    //当前为导入成功的
                    if (notImport == null)
                    {
                        //当前案例为曾经导入失败的案例
                        if (caseObj.是否已进行入库整理 == 1 && countObj.NotImportCaseCount > 0)
                        {
                            countObj.NotImportCaseCount = countObj.NotImportCaseCount - 1;
                        }
                    }
                    else//当前为导入失败的
                    {
                        //当前案例为曾经导入失败的案例
                        if (caseObj.是否已进行入库整理 != 1)
                        {
                            countObj.NotImportCaseCount = countObj.NotImportCaseCount + 1;
                        }
                    }
                    if (nowObj)
                    {
                        addCount.Add(countObj);
                    }
                }
                db.DB.SysData_ProjectCaseCount.InsertAllOnSubmit(addCount);
                db.DB.SubmitChanges();
            }
            catch (Exception ex)
            {
                db.Connection_Close();
                db.Dispose();
                message = "导入案例后统计未入库案例个数_系统异常";
                log.Error("UpdateNotImportCaseCount(List<VIEW_案例信息_城市表_网站表> importCaseList, List<案例库上传信息过滤表> notImportCase,out string message, DataClass _db = null)", ex);

                return false;
            }
            db.Connection_Close();
            db.Dispose();
            return true;
        }
        //public static void test()
        //{
        //    string message = "";
        //    DataClass dc=new DataClass();

        //    List<VIEW_案例信息_城市表_网站表> list1 = dc.DB.VIEW_案例信息_城市表_网站表.Where(p => p.ID == 338026).ToList();
        //    List<案例库上传信息过滤表> notImportCase1 = new List<案例库上传信息过滤表>();
        //    案例库上传信息过滤表 obj1 = new 案例库上传信息过滤表() { 案例ID = 338026 };
        //    notImportCase1.Add(obj1);
        //    UpdateNotImportCaseCount(list1, notImportCase1, out message, _db: dc);

        //    List<VIEW_案例信息_城市表_网站表> list2 = dc.DB.VIEW_案例信息_城市表_网站表.Where(p => p.ID == 338027).ToList();
        //    List<案例库上传信息过滤表> notImportCase2 = new List<案例库上传信息过滤表>();
        //    案例库上传信息过滤表 obj2 = new 案例库上传信息过滤表() { 案例ID = 338027 };
        //    notImportCase2.Add(obj2);
        //    UpdateNotImportCaseCount(list2, notImportCase2, out message, _db: dc);

        //    List<VIEW_案例信息_城市表_网站表> list3 = dc.DB.VIEW_案例信息_城市表_网站表.Where(p => p.ID == 338028).ToList();
        //    List<案例库上传信息过滤表> notImportCase3 = new List<案例库上传信息过滤表>();
        //    案例库上传信息过滤表 obj3 = new 案例库上传信息过滤表() { 案例ID = 338028 };
        //    notImportCase3.Add(obj3);
        //    UpdateNotImportCaseCount(list3, notImportCase3, out message, _db: dc);

        //    dc.Connection_Close();
        //    dc.Dispose();
        //}
    }
}
