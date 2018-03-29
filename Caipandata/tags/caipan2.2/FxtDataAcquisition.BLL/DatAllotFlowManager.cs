using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class DatAllotFlowManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DatAllotFlowManager));
        #region (查询)
        /// <summary>
        /// 根据城市+用户ID+状态+多个楼盘ID获取
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="userName">当前用户名</param>
        /// <param name="projectIds">楼盘ID</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DatAllotFlow> GetDatAllotFlowByStatusAndProjectIds(int cityId, string userName, int[] status, int[] projectIds, DataBase _db = null)
        {
            if (projectIds == null || projectIds.Length < 1 || status == null || status.Length < 1)
            {
                return new List<DatAllotFlow>();
            }
            DataBase db = new DataBase(_db);

            string sql = "{0} CityId=:cityId and  DatType=:datType and StateCode in ({1}) and DatId in ({2}) and SurveyUserName=:userName ";
            sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotFlow), status.ConvertToString(), projectIds.ConvertToString());

            List<NHParameter> parameters = new List<NHParameter>();
            parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
            parameters.Add(new NHParameter("datType", SYSCodeManager.DATATYPECODE_1, NHibernateUtil.Int32));
            parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
            IList<DatAllotFlow> list = db.DB.GetCustomSQLQueryList<DatAllotFlow>(sql, parameters);
            db.Close();
            return list;
        }
        /// <summary>
        /// 根据id获取任务信息
        /// </summary>
        /// <param name="Id">任务ID</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DatAllotFlow GetDatAllotFlowById(long Id, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} id=:Id";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotFlow));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("Id", Id, NHibernateUtil.Int64));
                DatAllotFlow obj = db.DB.GetCustomSQLQueryEntity<DatAllotFlow>(sql, parameters);

                //DatAllotFlow obj = db.DB.GetCustom<DatAllotFlow>((Expression<Func<DatAllotFlow, bool>>)(tbl => tbl.id == Id));
                db.Close();
                return obj;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据多个任务ID获取任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DatAllotFlow> GetDatAllotFlowByIds(long[] id, DataBase _db = null)
        {
            if (id == null || id.Length < 1)
            {
                return new List<DatAllotFlow>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} id in (" + id.ConvertToString() + ")";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotFlow));
                IList<DatAllotFlow> list = db.DB.GetCustomSQLQueryList<DatAllotFlow>(sql, null);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据用户和任务ID获取任务信息
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <param name="userName">当前用户名</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DatAllotFlow GetDatAllotFlowByIdAndUserId(long id, string userName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} id =:id and (UserName =:userName or SurveyUserName=:userName)";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotFlow));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("id", id, NHibernateUtil.Int64));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                DatAllotFlow obj = db.DB.GetCustomSQLQueryEntity<DatAllotFlow>(sql, parameters);
                //DatAllotFlow obj = db.DB.GetCustom<DatAllotFlow>((Expression<Func<DatAllotFlow, bool>>)(tbl => tbl.id == id && tbl.UserName == userName));
                db.Close();
                return obj;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DatAllotFlow GetDatAllotFlowByIdAndUserIdAndCityId(long id, string userName, int cityId, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} id =:id and CityId = :cityId and (UserName =:userName or SurveyUserName=:userName)";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotFlow));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("id", id, NHibernateUtil.Int64));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                DatAllotFlow obj = db.DB.GetCustomSQLQueryEntity<DatAllotFlow>(sql, parameters);

                //DatAllotFlow obj = db.DB.GetCustom<DatAllotFlow>((Expression<Func<DatAllotFlow, bool>>)(tbl => tbl.id == id && tbl.SurveyUserName == userName && tbl.CityId == cityId));
                db.Close();
                return obj;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <param name="cityId">当前城市ID</param>
        /// <param name="companyId">当前企业ID</param>
        /// <param name="userName">当前用户</param>
        /// <param name="areaId">行政区ID(可选)</param>
        /// <param name="stateCode">任务状态(可选)</param>
        /// <param name="startDate">时间区间_开始(可选)</param>
        /// <param name="endDate">时间区间_结束(可选)</param>
        /// <param name="departmentId">所在小组(可选)</param>
        /// <param name="functionCodes">当前用户对任务所拥有的操作权限</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<View_AllotFlowJoinProject> GetDatAllotFlowProjectSearch(int cityId, int companyId, string userName, string projectName, int? areaId,
            int? stateCode, DateTime? startDate, DateTime? endDate, int[] functionCodes, int pageIndex,
            int pageSize, out int count, bool isGetCount = true, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            try
            {

                count = 0;
                StringBuilder sbSql = new StringBuilder();
                List<NHParameter> parameters = new List<NHParameter>();
                sbSql.Append(string.Format("{0} AllotFxtCompanyId=:companyId and AllotCityId=:cityId and DatType=:datType and Valid=1", NHibernateUtility.GetMSSQL_SQL_VIEW(NHibernateUtility.ViewName_DatAllotFlowJoinProject)));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("datType", SYSCodeManager.DATATYPECODE_1, NHibernateUtil.Int32));
                //楼盘名称
                if (!string.IsNullOrEmpty(projectName))
                {
                    //projectName = "'%" + projectName + "%'";
                    sbSql.Append(" and ProjectName like N'%'+:projectName+'%' ");
                    parameters.Add(new NHParameter("projectName", projectName, NHibernateUtil.String));
                }
                //根据行政区
                if (areaId != null)
                {
                    sbSql.Append(" and AreaID=:areaId");
                    parameters.Add(new NHParameter("areaId", Convert.ToInt32(areaId), NHibernateUtil.Int32));
                }   //根据状态
                if (stateCode != null)
                {
                    sbSql.Append(" and StateCode=:stateCode");
                    parameters.Add(new NHParameter("stateCode", stateCode, NHibernateUtil.Int32));
                }
                //根据开始时间
                if (startDate != null)
                {
                    sbSql.Append(" and StateDate>=:startDate ");
                    parameters.Add(new NHParameter("startDate", Convert.ToDateTime(Convert.ToDateTime(startDate).ToString("yyyy-MM-dd 00:00:00")), NHibernateUtil.DateTime));
                }
                //根据结束时间
                if (endDate != null)
                {
                    sbSql.Append(" and StateDate<=:endDate");
                    parameters.Add(new NHParameter("endDate", Convert.ToDateTime(Convert.ToDateTime(endDate).ToString("yyyy-MM-dd 23:59:59")), NHibernateUtil.DateTime));
                }
                //根据操作权限
                if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_3))//查看公司全部(管理员+分配人+审核人)
                {
                }
                else if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_2))//查看小组内(组长)
                {
                    sbSql.Append(string.Format(" and (UserName=:userName or SurveyUserName=:userName or UserName in (select UserName from {0} with(nolock) where DepartmentID in (select DepartmentID from {0} with(nolock) where  CityID=:cityId and FxtCompanyID=:companyId and UserName=:userName  and DepartmentID in (select DepartmentID from {1} with(nolock) where DValid=1))))", NHibernateUtility.TableName_PriviDepartmentUser, NHibernateUtility.TableName_PriviDepartment));
                    parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                }
                else//查看自己(查勘员)
                {
                    sbSql.Append(" and (UserName=:userName or SurveyUserName=:userName)");
                    parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                }

                UtilityPager pageInfo = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                IList<View_AllotFlowJoinProject> list = db.DB.PagerList<View_AllotFlowJoinProject>(pageInfo, sbSql.ToString(), parameters, " StateDate", "Desc", isDTO: true);
                count = pageInfo.Count;
                db.Close();
                return list;

            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #endregion

        #region (更新)
        /// <summary>
        /// 将任务设置为查勘中
        /// </summary>
        /// <param name="userName">当前用户名</param>
        /// <param name="cityid">当前城市ID</param>
        /// <param name="allotid">当前任务ID</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns>1:成功,0:失败,-1:失败,系统异常</returns>
        public static int SetAllotSurveyingStatus(string userName, int cityid, long allotid, out string message, DataBase _db = null)
        {
            message = "";
            DataBase db = new DataBase(_db);
            try
            {

                DatAllotFlow allotObj = GetDatAllotFlowByIdAndUserId(allotid, userName, db);
                if (allotObj == null)
                {
                    db.Close();
                    message = "任务不存在";
                    return 0;
                }
                if (allotObj.DatType == SYSCodeManager.DATATYPECODE_1)
                {
                    DATProject projObj = DATProjectManager.GetProjectByProjectId(Convert.ToInt32(allotObj.DatId), allotObj.CityId, db);
                    if (projObj == null)
                    {
                        db.Close();
                        message = "任务不存在";
                        return 0;
                    }
                    using (ITransaction tx = db.DB.BeginTransaction())
                    {
                        try
                        {
                            if (allotObj.StateCode != SYSCodeManager.STATECODE_4)
                            {
                                allotObj.StateCode = SYSCodeManager.STATECODE_4;
                                allotObj.StateDate = DateTime.Now;
                                db.DB.Update(allotObj, tx);
                                DatAllotSurveyManager.InsertAllotSurvey(allotid, allotObj.CityId, allotObj.FxtCompanyId, userName, SYSCodeManager.STATECODE_4, DateTime.Now, db, tx);
                            }
                            if (projObj.Status != SYSCodeManager.STATECODE_4)
                            {
                                projObj.Status = SYSCodeManager.STATECODE_4;
                                projObj.SaveUser = userName;
                                projObj.SaveDateTime = DateTime.Now;
                                db.DB.Update(projObj, tx);
                            }
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            db.Close();
                            message = "系统异常";
                            return -1;
                        }
                    }

                }
                db.Close();
                return 1;

            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 自审通过
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark">自审说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotMyThroughAudit(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择已查勘的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_5).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择已查勘的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_6, "<span class=\"red\">自审通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark), out message, db, tx); ;
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        //设置自审信息
                        foreach (long _allotId in allotIds)
                        {
                            DatAllotFlow allotObj = allotList.Where(obj => obj.id == _allotId).FirstOrDefault();
                            result = DATCheckManager.SetMyCheckInfo(cityId, nowCompanyId, _allotId, allotObj.DatId, SYSCodeManager.DATATYPECODE_1, userName,
                                true, remark, out message, db, tx);
                            if (result != 1)
                            {
                                tx.Rollback();
                                db.Close();
                                return result;
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        log.Error("自审通过异常", ex);
                        return -1;
                    }
                }
                db.Close();
                return 1;

            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 自审不通过
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark">自审说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotMyNotThroughAudit(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择已查勘的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_5).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择已查勘的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_7, "<span class=\"red\">自审不通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark) + "<br/>任务已设置为<span class=\"red\">待查勘</span>", out message, db, tx);
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        //设置自审信息
                        foreach (long _allotId in allotIds)
                        {
                            DatAllotFlow allotObj = allotList.Where(obj => obj.id == _allotId).FirstOrDefault();
                            result = DATCheckManager.SetMyCheckInfo(cityId, nowCompanyId, _allotId, allotObj.DatId, SYSCodeManager.DATATYPECODE_1, userName,
                                false, remark, out message, db, tx);
                            if (result != 1)
                            {
                                tx.Rollback();
                                db.Close();
                                return result;
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        log.Error("自审不通过异常", ex);
                        return -1;
                    }
                }
                db.Close();
                return 1;

            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark">自审说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotThroughAudit(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择自审通过的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_6).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择自审通过的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_8, "<span class=\"red\">审核通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark), out message, db, tx);
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        //设置自审信息
                        foreach (long _allotId in allotIds)
                        {
                            DatAllotFlow allotObj = allotList.Where(obj => obj.id == _allotId).FirstOrDefault();
                            result = DATCheckManager.SetCheckInfo(cityId, nowCompanyId, _allotId, allotObj.DatId, SYSCodeManager.DATATYPECODE_1, userName,
                                true, remark, out message, db, tx);
                            if (result != 1)
                            {
                                tx.Rollback();
                                db.Close();
                                return result;
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        log.Error("审核通过异常", ex);
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark">自审说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotNotThroughAudit(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择自审通过的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_6).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择自审通过的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_9
                            , "<span class=\"red\">审核不通过</span>" + (remark.IsNullOrEmpty()? "" : ",说明:" + remark) + "<br/>任务已设置为<span class=\"red\">已查勘</span>", out message, db, tx);
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        //设置自审信息
                        foreach (long _allotId in allotIds)
                        {
                            DatAllotFlow allotObj = allotList.Where(obj => obj.id == _allotId).FirstOrDefault();
                            result = DATCheckManager.SetCheckInfo(cityId, nowCompanyId, _allotId, allotObj.DatId, SYSCodeManager.DATATYPECODE_1, userName,
                                false, remark, out message, db, tx);
                            if (result != 1)
                            {
                                tx.Rollback();
                                db.Close();
                                return result;
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        log.Error("审核不通过异常", ex);
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 从任何状态直接撤销到已分配状态(待查勘)-(用于自审不通过or审核不通过时)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotNotSurvey(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_2, remark, out message, db, tx);
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 从任何状态直接撤销到已查勘状态-(用于自审不通过or审核不通过时)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="remark"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int SetAllotIsSurvey(string userName, string userTrueName, int nowCompanyId, int cityId, long[] allotIds, string remark, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {
                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        int result = SetAllotStatus(userName, userTrueName, nowCompanyId, cityId, allotList, SYSCodeManager.STATECODE_5, remark, out message, db, tx);
                        if (result != 1)
                        {
                            tx.Rollback();
                            db.Close();
                            return result;
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="nowUserName"></param>
        /// <param name="nowCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="allotList"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns>1:成功,0:失败,-1:失败,系统异常</returns>
        public static int SetAllotStatus(string nowUserName, string userTrueName, int nowCompanyId, int cityId, IList<DatAllotFlow> allotList, int status, string remark, out string message, DataBase _db = null, ITransaction tx = null)
        {
            message = "";
            DataBase db = new DataBase(_db);
            try
            {
                TransactionHelper th = new TransactionHelper(db.DB, tx);
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择任务";
                    return 0;
                }
                try
                {
                    IList<DatAllotSurvey> addList = new List<DatAllotSurvey>();
                    List<int> projectIds = new List<int>();
                    for (int i = 0; i < allotList.Count; i++)
                    {
                        projectIds.Add(Convert.ToInt32(allotList[i].DatId));
                        if (SYSCodeManager.STATECODE_7 == status)
                        {
                            allotList[i].StateCode = SYSCodeManager.STATECODE_2;
                        }
                        else if (SYSCodeManager.STATECODE_9 == status)
                        {
                            allotList[i].StateCode = SYSCodeManager.STATECODE_5;
                        }
                        else
                        {
                            allotList[i].StateCode = status;
                        }

                        allotList[i].StateDate = DateTime.Now;
                        DatAllotSurvey obj = DatAllotSurveyManager.GetDatAllotSurveyInsertEntitie(allotList[i].id, allotList[i].CityId, nowCompanyId, nowUserName, status, DateTime.Now, remark: remark);
                        obj.TrueName = userTrueName;
                        addList.Add(obj);
                    }
                    IList<DATProject> projectlist = DATProjectManager.GetProjectByProjectIds(projectIds.ToArray(), db);
                    for (int i = 0; i < projectlist.Count; i++)
                    {
                        if (SYSCodeManager.STATECODE_7 == status)
                        {
                            projectlist[i].Status = SYSCodeManager.STATECODE_2;
                        }
                        else if (SYSCodeManager.STATECODE_9 == status)
                        {
                            projectlist[i].Status = SYSCodeManager.STATECODE_5;
                        }
                        else
                        {
                            projectlist[i].Status = status;
                        }
                        projectlist[i].SaveDateTime = DateTime.Now;
                        projectlist[i].SaveUser = nowUserName;
                    }
                    //更新任务信息和状态
                    db.DB.Update<DatAllotFlow>(allotList, tx);
                    //更新楼盘状态
                    db.DB.Update<DATProject>(projectlist, tx);
                    //插入状态更新记录
                    db.DB.Create<DatAllotSurvey>(addList, tx);
                    th.Commit();
                }
                catch (Exception ex)
                {
                    th.Rollback();
                    db.Close();
                    message = "系统异常";
                    return -1;
                }

                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="nowUserName">当前用户</param>
        /// <param name="nowCompanyId">当前机构</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="allotIds">需分配的任务ID</param>
        /// <param name="surveyUserName">需分配给的查勘员</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns>0:失败,1:成功,-1:系统异常</returns>
        public static int AssignAllotFlow(string nowUserName, string TrueName, int nowCompanyId, int cityId, long[] allotIds, string surveyUserName, string surveyUserTrueName, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择待分配的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {

                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_1).ToList();
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择待分配的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        IList<DatAllotSurvey> addList = new List<DatAllotSurvey>();
                        List<int> projectIds = new List<int>();
                        for (int i = 0; i < allotList.Count; i++)
                        {
                            projectIds.Add(Convert.ToInt32(allotList[i].DatId));
                            allotList[i].StateCode = SYSCodeManager.STATECODE_2;
                            allotList[i].StateDate = DateTime.Now;
                            allotList[i].SurveyUserName = surveyUserName;
                            allotList[i].SurveyUserTrueName = surveyUserTrueName;
                            allotList[i].UserName = nowUserName;
                            allotList[i].UserTrueName = TrueName;
                            DatAllotSurvey obj = DatAllotSurveyManager.GetDatAllotSurveyInsertEntitie(allotList[i].id, allotList[i].CityId, nowCompanyId, nowUserName, SYSCodeManager.STATECODE_2, DateTime.Now);
                            addList.Add(obj);

                        }
                        IList<DATProject> projectlist = DATProjectManager.GetProjectByProjectIds(projectIds.ToArray(), db);
                        for (int i = 0; i < projectlist.Count; i++)
                        {
                            projectlist[i].Status = SYSCodeManager.STATECODE_2;
                        }
                        //更新任务信息和状态
                        db.DB.Update<DatAllotFlow>(allotList, tx);
                        //更新楼盘状态
                        db.DB.Update<DATProject>(projectlist, tx);
                        //插入状态更新记录
                        db.DB.Create<DatAllotSurvey>(addList, tx);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 撤销任务(从已分配撤销到待分配)
        /// </summary>
        /// <param name="nowUserName">当前用户</param>
        /// <param name="nowCompanyId">当前机构</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="allotIds">需撤销的任务ID</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int CancelAllotFlow(string nowUserName, int nowCompanyId, int cityId, long[] allotIds, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择已分配的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {
                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_2).ToList(); ;
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择已分配的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        IList<DatAllotSurvey> addList = new List<DatAllotSurvey>();
                        List<int> projectIds = new List<int>();
                        for (int i = 0; i < allotList.Count; i++)
                        {
                            projectIds.Add(Convert.ToInt32(allotList[i].DatId));
                            allotList[i].StateCode = SYSCodeManager.STATECODE_1;
                            allotList[i].StateDate = DateTime.Now;
                            allotList[i].SurveyUserName = null;
                            allotList[i].SurveyUserTrueName = null;
                            DatAllotSurvey obj = DatAllotSurveyManager.GetDatAllotSurveyInsertEntitie(allotList[i].id, allotList[i].CityId, nowCompanyId, nowUserName, SYSCodeManager.STATECODE_1, DateTime.Now, remark: "撤销任务");
                            addList.Add(obj);
                        }
                        IList<DATProject> projectlist = DATProjectManager.GetProjectByProjectIds(projectIds.ToArray(), db);
                        for (int i = 0; i < projectlist.Count; i++)
                        {
                            projectlist[i].Status = SYSCodeManager.STATECODE_1;
                        }
                        //更新任务信息和状态
                        db.DB.Update<DatAllotFlow>(allotList, tx);
                        //更新楼盘状态
                        db.DB.Update<DATProject>(projectlist, tx);
                        //插入状态更新记录
                        db.DB.Create<DatAllotSurvey>(addList, tx);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 撤销查勘(从查勘中撤销到待查勘)
        /// </summary>
        /// <param name="nowUserName">当前用户</param>
        /// <param name="nowCompanyId">当前机构</param>
        /// <param name="cityId"当前城市></param>
        /// <param name="allotIds">需撤销的任务ID</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int CancelAllotFlowSurvey(string nowUserName, int nowCompanyId, int cityId, long[] allotIds, out string message, DataBase _db = null)
        {
            message = "";
            if (allotIds == null || allotIds.Length < 1)
            {
                message = "请选择查勘中的任务";
                return 0;
            }
            DataBase db = new DataBase(_db);
            try
            {
                IList<DatAllotFlow> allotList = GetDatAllotFlowByIds(allotIds, db).Where(obj => obj.StateCode == SYSCodeManager.STATECODE_4).ToList(); ;
                if (allotList == null || allotList.Count < 1)
                {
                    message = "请选择查勘中的任务";
                    return 0;
                }
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        IList<DatAllotSurvey> addList = new List<DatAllotSurvey>();
                        List<int> projectIds = new List<int>();
                        for (int i = 0; i < allotList.Count; i++)
                        {
                            projectIds.Add(Convert.ToInt32(allotList[i].DatId));
                            allotList[i].StateCode = SYSCodeManager.STATECODE_2;
                            allotList[i].StateDate = DateTime.Now;
                            DatAllotSurvey obj = DatAllotSurveyManager.GetDatAllotSurveyInsertEntitie(allotList[i].id, allotList[i].CityId, nowCompanyId, nowUserName, SYSCodeManager.STATECODE_2, DateTime.Now, remark: "撤销查勘-(手机端)");
                            addList.Add(obj);
                        }
                        IList<DATProject> projectlist = DATProjectManager.GetProjectByProjectIds(projectIds.ToArray(), db);
                        for (int i = 0; i < projectlist.Count; i++)
                        {
                            projectlist[i].Status = SYSCodeManager.STATECODE_2;
                        }
                        //更新任务信息和状态
                        db.DB.Update<DatAllotFlow>(allotList, tx);
                        //更新楼盘状态
                        db.DB.Update<DATProject>(projectlist, tx);
                        //插入状态更新记录
                        db.DB.Create<DatAllotSurvey>(addList, tx);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        db.Close();
                        message = "系统异常";
                        return -1;
                    }
                }
                db.Close();
                return 1;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #endregion
        #region Excel

        /// <summary>
        /// 导入任务
        /// </summary>
        /// <param name="project"></param>
        /// <param name="companyList"></param>
        /// <returns></returns>
        public static int ImportTask(DATProject project, List<LNKPCompany> companyList, DatAllotFlow flow, LNKPAppendage parkingStatus, DataBase _db = null)
        {
            int msg = 0;
            DataBase db = new DataBase(_db);
            try
            {
                using (ITransaction tran = db.DB.BeginTransaction())
                {
                    try
                    {
                        bool bl = db.DB.Create(project, tran);//添加楼盘
                        if (bl)
                        {
                            foreach (LNKPCompany company in companyList)
                            {
                                company.LNKPCompanyPX.ProjectId = project.ProjectId;
                                db.DB.Create(company, tran);
                            }
                            if (parkingStatus.AppendageCode != 0)
                            {
                                parkingStatus.ProjectId = project.ProjectId;
                                db.DB.Create(parkingStatus, tran);
                            }
                            flow.DatId = project.ProjectId;
                            db.DB.Create(flow, tran);
                            DatAllotSurvey survey = new DatAllotSurvey();
                            survey.AllotId = flow.id;
                            survey.CityId = flow.CityId;
                            survey.FxtCompanyId = flow.FxtCompanyId;
                            survey.UserName = flow.UserName;
                            survey.TrueName = flow.UserTrueName;
                            survey.CreateDate = DateTime.Now;
                            survey.StateCode = flow.StateCode;
                            survey.StateDate = DateTime.Now;
                            survey.Remark = "excel导入任务！";
                            db.DB.Create(survey, tran);
                        }
                        tran.Commit();
                        msg = 1;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        msg = -1;
                    }
                }
                db.Close();
                return msg;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取单个楼盘信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        private static DATProject GetProject(int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {

                StringBuilder sbSql = new StringBuilder();
                List<NHParameter> parameters = new List<NHParameter>();
                sbSql.AppendFormat(" {0} ProjectId=:ProjectId ", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATProject));
                //            sbSql.Append(@"select ProjectId,FxtProjectId,ProjectName,OtherName,Address,AreaID,SubAreaId,PurposeCode,RightCode		
                //                ,FieldNo,StartDate,UsableYear,BuildingTypeCode,CubageRate,GreenRate,LandArea,BuildingArea		
                //                ,EndDate,SaleDate,BuildingDate,JoinDate,SalePrice,BuildingNum,TotalNum,ParkingNumber	
                //                ,ManagerPrice,ManagerTel,Detail,East,West,South,North,X,Y
                //                from FxtTemp.dbo.DAT_Project where  ");
                //            sbSql.Append(" ProjectId=:ProjectId ");
                parameters.Add(new NHParameter("ProjectId", projectId, NHibernateUtil.Int32));
                DATProject project = db.DB.GetCustomSQLQueryEntity<DATProject>(sbSql.ToString(), parameters);
                db.Close();
                return project;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        private static IList<LNKPCompany> GetLNKPCompanyList(int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sbSql = new StringBuilder();
                List<NHParameter> parameters = new List<NHParameter>();
                sbSql.AppendFormat(" {0} ProjectId=:ProjectId ", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPCompany));
                //            sbSql.Append(@"select * from FxtTemp.dbo.LNK_P_Company
                //                where ");
                //            sbSql.Append(@" ProjectId=:ProjectId ");
                parameters.Add(new NHParameter("ProjectId", projectId, NHibernateUtil.Int32));
                IList<LNKPCompany> companylist = db.DB.GetCustomSQLQueryList<LNKPCompany>(sbSql.ToString(), parameters);
                db.Close();
                return companylist;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取楼栋列表信息
        /// </summary>
        /// <param name="project"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        private static IList<DATBuilding> GetBuildingList(int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sbSql = new StringBuilder();
                List<NHParameter> parameters = new List<NHParameter>();
                sbSql.AppendFormat(" {0} ProjectId=:ProjectId ", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATBuilding));
                //            sbSql.Append(@"select ProjectId,BuildingId,BuildingName,Doorplate,PurposeCode,BuildingTypeCode
                //                ,StructureCode,BuildDate,SaleDate,SalePrice,UnitsNumber,TotalFloor,TotalNumber
                //                ,TotalBuildArea,OtherName,ElevatorRate,IsElevator,Wall,SaleLicence,LicenceDate
                //                ,LocationCode,FrontCode,SightCode,Weight,PriceDetail,Remark
                //                from FxtTemp.dbo.DAT_Building
                //                where ");
                //            sbSql.Append(@" ProjectId=:ProjectId ");
                parameters.Add(new NHParameter("ProjectId", projectId, NHibernateUtil.Int32));
                IList<DATBuilding> buildlist = db.DB.GetCustomSQLQueryList<DATBuilding>(sbSql.ToString(), parameters);
                db.Close();
                return buildlist;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        private static IList<DATHouse> GetHouseList(int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sbSql = new StringBuilder();
                List<NHParameter> parameters = new List<NHParameter>();
                sbSql.AppendFormat(" {0} BuildingId in (select BuildingId from FxtTemp.dbo.DAT_Building where ProjectId=:ProjectId) ",
                    NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse));
                //            sbSql.Append(@"select BuildingId,HouseId,HouseName,FloorNo,UnitNo,BuildArea
                //                ,'' as 套内面积,HouseTypeCode,StructureCode,SalePrice,UnitPrice
                //                ,FrontCode,SightCode,PurposeCode,Weight
                //                from FxtTemp.dbo.DAT_House
                //                where BuildingId in 
                //                (select BuildingId from FxtTemp.dbo.DAT_Building where ProjectId=:ProjectId) ");
                parameters.Add(new NHParameter("ProjectId", projectId, NHibernateUtil.Int32));
                IList<DATHouse> houselist = db.DB.GetCustomSQLQueryList<DATHouse>(sbSql.ToString(), parameters);
                db.Close();
                return houselist;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 导入数据到运维中心
        /// </summary>
        /// <param name="allowId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int ImportToDataCenter(long allowId, int cityId, int companyId, string username, string signname, List<UserCenter_Apps> appList, out string message, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            message = "";
            try
            {
                DatAllotFlow allot = GetDatAllotFlowById(allowId, db);
                if (allot == null || allot.StateCode != SYSCodeManager.STATECODE_8)
                {
                    db.Close();
                    message = "入库失败:该任务不存在或者该任务状态不为审核通过";
                    return 0;
                }
                if (allot.DatType == SYSCodeManager.DATATYPECODE_1)
                {
                    DATProject proj = DATProjectManager.GetProjectByProjectId(Convert.ToInt32(allot.DatId), cityId, db);
                    if (proj == null)
                    {
                        db.Close();
                        message = "入库失败:楼盘不存在";
                        return 0;
                    }
                    IList<LNKPCompany> lnkpc = LNKPCompanyManager.GetLNKPCompanyByProjectId(cityId, proj.ProjectId, db);
                    IList<LNKPAppendage> lnkpa = LNKPAppendageManager.GetLNKPAppendageByProjectId(cityId, proj.ProjectId, db);
                    IList<DATBuilding> buildingList = DATBuildingManager.GetBuildingByProjectId(proj.ProjectId, cityId, db);
                    IList<DATHouse> houseList = DATHouseManager.GetHouseByProjectId(proj.ProjectId, cityId, db);
                    IList<DATHouse> copyHouseList = new List<DATHouse>();//用于传给数据中心存储的
                    //批量生产房号
                    foreach (DATHouse house in houseList)
                    {
                        /////////获取单元号+室号////////////
                        string unitNoStr = house.UnitNo;
                        int startFloor = house.FloorNo;
                        int endFloor = Convert.ToInt32(house.EndFloorNo);
                        if (house.EndFloorNo != null && Convert.ToInt32(house.EndFloorNo) < house.FloorNo)//输入的结束楼层小于起始楼层则调过来递增
                        {
                            startFloor = Convert.ToInt32(house.EndFloorNo);
                            endFloor = house.FloorNo;
                        }
                        string unitNo = DATHouseManager.GetUnitNoByUnitNoStr(house.UnitNo);
                        string houseNo = DATHouseManager.GetHouseNoByUnitNoStr(house.UnitNo);
                        //////循环生成房号//////////////////
                        int nowFloor = house.FloorNo;
                        for (int i = startFloor; (i <= Convert.ToInt32(endFloor) && house.EndFloorNo != null) || i == startFloor; i++)
                        {
                            DATHouse _house = house.CopyEntity<DATHouse>();
                            _house.UnitNo = unitNo + houseNo;
                            _house.FloorNo = i;
                            _house.HouseName = unitNo + i + houseNo;

                            if (copyHouseList.Where(obj => obj.BuildingId == _house.BuildingId && obj.FloorNo == _house.FloorNo && obj.HouseName == _house.HouseName).FirstOrDefault() == null)
                            {
                                copyHouseList.Add(_house);
                            }
                        }
                    }
                    int fxtprojectId = 0;
                    //DataCenterProjectApi.ImportProjectData(proj, lnkpc, lnkpa, buildingList, copyHouseList, username, signname, appList, out message);
                    if (fxtprojectId <= 0)
                    {
                        db.Close();
                        message = "入库失败:导入楼盘信息异常";
                        return 0;
                    }
                    proj.FxtProjectId = fxtprojectId;
                    db.DB.Update(proj);
                    //上传照片
                    IList<LNKPPhoto> photoList = LNKPPhotoManager.GetLNKPPhotoByProjectId(proj.ProjectId, cityId, companyId, db);
                    foreach (LNKPPhoto pObj in photoList)
                    {
                        FileStream fStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath(pObj.Path), FileMode.Open, FileAccess.Read);
                        BinaryReader bReader = new BinaryReader(fStream);//将文件了加载成二进制数据
                        long length = fStream.Length;//当前文件的总大小
                        //创建一个用于存储要上传文件内容的字节对象
                        byte[] data = new byte[length];
                        //将流中读取指定字节数加载到到字节对象data
                        bReader.Read(data, 0, Convert.ToInt32(length));
                        //if (DataCenterProjectApi.AddProjectPhoto(fxtprojectId, cityId, Convert.ToInt32(pObj.PhotoTypeCode), new FileInfo(System.Web.HttpContext.Current.Server.MapPath(pObj.Path)).Name, pObj.PhotoName, data, username, signname, appList, out message) != 1)
                        //{
                        //    db.Close();
                        //    message = "入库失败:上传楼盘照片失败(id:" + pObj.Id + ")";
                        //    return 0;
                        //}
                    }
                    //设置为已入库
                    proj.Status = SYSCodeManager.STATECODE_10;
                    db.DB.Update(proj);
                }
                //设置为已入库
                allot.StateCode = SYSCodeManager.STATECODE_10;
                allot.StateDate = DateTime.Now;
                db.DB.Update(allot);
                //记录日志
                DatAllotSurveyManager.InsertAllotSurvey(allowId, cityId, companyId, username, SYSCodeManager.STATECODE_10, DateTime.Now, db);
            }
            catch (Exception ex)
            {
                log.Error("入库楼盘系统异常(allot:" + allowId.ToString() + "", ex);
                db.Close();
                message = "入库楼盘系统异常";
                return 0;

            }
            db.Close();
            return 1;

        }

        /// <summary>
        /// 导出任务
        /// </summary>
        /// <param name="project"></param>
        /// <param name="building"></param>
        /// <param name="house"></param>
        public static void ExportTaskDetail(int projectid, out DATProject project, out IList<LNKPCompany> companyList,
            out IList<DATBuilding> building, out IList<DATHouse> house, DataBase _db = null)
        {
            companyList = null;
            building = null;
            house = null;
            project = GetProject(projectid);
            if (project != null)
            {
                companyList = GetLNKPCompanyList(projectid);
                building = GetBuildingList(projectid);
                house = GetHouseList(projectid);
            }
        }
        /// <summary>
        /// 导入任务
        /// </summary>
        /// <param name="project"></param>
        /// <param name="building"></param>
        /// <param name="house"></param>
        public static int ImportTaskDetail(DATProject project, List<DATBuilding> buildingList, List<DATHouse> houseList,
            List<LNKPCompany> companyList, DataBase _db = null)
        {
            int msg = 0;
            DataBase db = new DataBase(_db);
            try
            {

                using (ITransaction tran = db.DB.BeginTransaction())
                {
                    try
                    {
                        bool bl = db.DB.Update(project, tran);//添加楼盘
                        if (bl)
                        {
                            foreach (LNKPCompany company in companyList)
                            {
                                company.LNKPCompanyPX.ProjectId = project.ProjectId;
                                db.DB.Update(company, tran);
                            }
                            //楼栋
                            foreach (DATBuilding building in buildingList)
                            {
                                db.DB.Update(building, tran);
                            }
                            //房号
                            foreach (DATHouse house in houseList)
                            {
                                db.DB.Update(house, tran);
                            }
                            //DatAllotSurvey survey = new DatAllotSurvey();
                            //survey.AllotId = flow.id;
                            //survey.CityId = flow.CityId;
                            //survey.FxtCompanyId = flow.FxtCompanyId;
                            //survey.UserName = flow.UserName;
                            //survey.CreateDate = DateTime.Now;
                            //survey.StateCode = flow.StateCode;
                            //survey.StateDate = DateTime.Now;
                            //survey.Remark = "excel导入任务！";
                            //db.DB.Create(survey, tran);
                        }
                        tran.Commit();
                        msg = 1;
                    }
                    catch (Exception ex)
                    {

                        tran.Rollback();
                        msg = -1;
                    }
                }
                db.Close();
                return msg;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        #endregion
    }
}
