using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DATProjectDomain.Entities;
using FxtNHibernate.DTODomain.APIActualizeDTO;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtNHibernater.Data;
using FxtService.Common;
using FxtService.Contract.APIInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace FxtService.Service.APIActualize
{
    public class FxtTask : IFxtTask
    {
        /// <summary>
        /// 任务修改
        /// </summary>
        /// <param name="data">任务模型</param>
        /// <returns></returns>
        public ResultData TaskUpdate(string data)
        {
            SysTask sysTask = Utils.Deserialize<SysTask>(data);
            sysTask.EndDateTime = DateTime.Now;
            sysTask.Status = 1;
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysTask>(sysTask);
            if (objId > 0)
            {
                return Utility.GetObjJson(1, "成功");
            }
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 修改任务成功及失败条数
        /// </summary>
        /// <param name="data">任务模型</param>
        /// <returns></returns>
        public ResultData TaskSuccessFail(string data)
        {
            SysTask sysTask = Utils.Deserialize<SysTask>(data);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysTask>(sysTask);
            if (objId > 0)
            {
                return Utility.GetObjJson(1, "成功");
            }
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 根据任务ID获得任务
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        public ResultData GetTaskById(string id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string sql = string.Format("{0} Id={1}", Utility.GetMSSQL_SQL(typeof(SysTask), Utility.loand_Sys_Task), id);
            SysTask task = mssqlado.GetModel<SysTask>(sql);
            if (task != null)
                return Utility.GetObjJson(1, "Success", task);
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public ResultData TaskList()
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            //获取状态为开始、暂停的任务
            string sql = string.Format("{0} [Status]=0 or [Status]=2",
                Utility.GetMSSQL_SQL(typeof(SysTask), Utility.loand_Sys_Task));
            List<TaskToFile> list = mssqlado.GetList<TaskToFile>(sql);

            foreach (var item in list)
            {
                MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
                //文件地址
                SysUploadFile sysUploadFile = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysUploadFile>(item.UploadFileId);
                item.FileUrl = sysUploadFile != null ? sysUploadFile.FilePath : "";

                //银行所属项目名称
                SysBankProject sysBankProject = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysBankProject>(item.BankProjectId);
                item.BankProjectName = sysBankProject != null ? sysBankProject.ProjectName : "";

                //任务所属用户名称
                SysUser sysUser = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysUser>(item.UserId);
                item.UserName = sysUser != null ? sysUser.TrueName : "";

                //公司、银行、评估机构
                SysCustomer sysCustomer = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysCustomer>(item.BankId);
                item.BankName = sysCustomer != null ? sysCustomer.CustomerName : "";
            }
            if (list.Count > 0)
            {
                return Utility.GetObjJson(1, "成功", list);
            }
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 新增任务日志
        /// </summary>
        /// <param name="data">任务日志实体</param>
        /// <returns></returns>
        public ResultData TaskLogAdd(string data)
        {
            SysTaskLog sysTaskLog = Utils.Deserialize<SysTaskLog>(data);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysTaskLog>(sysTaskLog);
            if (flag > 0)
                return Utility.GetObjJson(1, "Success");
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 获取要复估的押品列表
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        public ResultData GetDataCollateral(string id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string sql = string.Format("{0} [Status]=1 and UploadFileId={1}",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral), id);

            List<DataCollateral> list = mssqlado.GetList<DataCollateral>(sql);
            if (list.Count() > 0)
            {
                return Utility.GetObjJson(1, "成功", list);
            }
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 押品拆分保存
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="uploadFileId">文件ID</param>
        /// <returns></returns>
        public ResultData DataCollateralAdd(string data)
        {
            DataCollateral collateral = Utils.Deserialize<DataCollateral>(data);
            if (collateral.MatchStatus != null)
                collateral.Status = 1;
            collateral.CreateDate = DateTime.Now;

            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataCollateral>(collateral);
            if (objId > 0)
            {
                return Utility.GetObjJson(1, "成功");
            }
            return Utility.GetObjJson(0, "");
        }

        /// <summary>
        /// 复估押品
        /// </summary>
        /// <param name="id">押品编号</param>
        /// <returns></returns>
        public ResultData RunCalculation(string id)
        {
            object objClass = System.Reflection.Assembly
                .Load("FxtService.Service")
                .CreateInstance("FxtService.Service.FxtLoanActualize.FxtCollaterals");
            MethodInfo method = objClass.GetType().GetMethod("ReassessmentCalculation");
            object result = method.Invoke(objClass, new object[] { Convert.ToInt32(id) });
            int type = Convert.ToInt32(Utils.GetJObjectValue(result.ToString(), "type"));
            string message = Utils.GetJObjectValue(result.ToString(), "message");
            return Utility.GetObjJson(type, message);
        }
    }
}
