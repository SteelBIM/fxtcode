using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class DATCheckManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATCheckManager));
        /// <summary>
        /// 根据任务ID获取任务审核信息
        /// </summary>
        /// <param name="allotId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DatCheck GetCheckByAllotId(long allotId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} allotId=:allotId order by Id desc";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatCheck, keyword: " top 1 "));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("allotId", allotId, NHibernateUtil.Int64));
                DatCheck obj = db.DB.GetCustomSQLQueryEntity<DatCheck>(sql, parameters);
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
        /// 设置自审信息
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="companyId">企业</param>
        /// <param name="allotId">任务ID</param>
        /// <param name="datType">任务数据类型</param>
        /// <param name="userName">自审人</param>
        /// <param name="pass">是否通过</param>
        /// <param name="remark">自审说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static int SetMyCheckInfo(int cityId, int companyId, long allotId, long datId, int datType, string userName, bool pass, string remark, out string message, DataBase _db = null, ITransaction tx = null)
        {
            message = "";
            DataBase db = new DataBase(_db);
            TransactionHelper tr = new TransactionHelper(db.DB, tx);
            try
            {
                bool add = false;
                DatCheck obj = GetCheckByAllotId(allotId, db);
                //是否存在数据
                if (obj == null)
                {
                    add = true;
                    obj = new DatCheck();
                    obj.CityId = cityId;
                    obj.FxtCompanyId = companyId;
                    obj.AllotId = allotId;
                    obj.DatType = datType;
                    obj.DatId = datId;
                }
                obj.CheckUserName1 = userName;
                obj.CheckState1 = SYSCodeManager.STATECODE_7;
                if (pass)
                {
                    obj.CheckState1 = SYSCodeManager.STATECODE_6;
                }
                obj.CheckRemark1 = remark;
                obj.CheckDate1 = DateTime.Now;
                bool upResult = false;
                if (add)
                {
                    upResult = db.DB.Create(obj, tx);
                }
                else
                {
                    upResult = db.DB.Update(obj, tx);
                }
                if (!upResult)
                {
                    tr.Rollback();
                    db.Close();
                    return 0;
                }
                tr.Commit();
                db.Close();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                db.Close();
                message = "系统异常";
                log.Error("SetMyCheckInfo:系统异常", ex);
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 设置审核信息
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="companyId">企业</param>
        /// <param name="allotId">任务ID</param>
        /// <param name="datType">任务数据类型</param>
        /// <param name="userName">审核人</param>
        /// <param name="pass">是否通过</param>
        /// <param name="remark">审核说明</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static int SetCheckInfo(int cityId, int companyId, long allotId, long datId, int datType, string userName, bool pass, string remark, out string message, DataBase _db = null, ITransaction tx = null)
        {
            message = "";
            DataBase db = new DataBase(_db);
            TransactionHelper tr = new TransactionHelper(db.DB, tx);
            try
            {
                bool add = false;
                DatCheck obj = GetCheckByAllotId(allotId, db);
                //是否存在数据
                if (obj == null)
                {
                    add = true;
                    obj = new DatCheck();
                    obj.CityId = cityId;
                    obj.FxtCompanyId = companyId;
                    obj.AllotId = allotId;
                    obj.DatType = datType;
                    obj.DatId = datId;
                    obj.CheckUserName1 = userName;
                    obj.CheckState1 = SYSCodeManager.STATECODE_6;
                    obj.CheckRemark1 = remark;
                    obj.CheckDate1 = DateTime.Now;
                }
                obj.CheckUserName2 = userName;
                obj.CheckState2 = SYSCodeManager.STATECODE_9;
                if (pass)
                {
                    obj.CheckState2 = SYSCodeManager.STATECODE_8;
                }
                obj.CheckRemark2 = remark;
                obj.CheckDate2 = DateTime.Now;
                bool upResult = false;
                if (add)
                {
                    upResult = db.DB.Create(obj, tx);
                }
                else
                {
                    upResult = db.DB.Update(obj, tx);
                }
                if (!upResult)
                {
                    tr.Rollback();
                    db.Close();
                    return 0;
                }
                tr.Commit();
                db.Close();
            }
            catch (Exception ex)
            {

                tr.Rollback();
                db.Close();
                message = "系统异常";
                log.Error("SetCheckInfo:系统异常", ex);
                return -1;
            }
            return 1;
        }
    }
}
