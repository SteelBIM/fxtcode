using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FxtCommonLibrary.LibraryUtils;
using CAS.Entity;
using CAS.Entity.BaseDAModels;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using CAS.Common;
using System.Web;
using System.IO;

/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 BaseController 控制器基类
 *  2014.06.11 修改人 曹青
 *             新增 wcf调用名称 客户、用户管理 CheckCustomerName、AddEditCustomer、GetCustomer、CheckUserName、AddEditUser、GetUser
 *  2014.06.17 修改人 曹青
 *             新增 wcf调用名称 UserLogin、ModifyPassword
 *  2014.06.18 修改人 曹青
 *             新增 wcf调用名称 DeleteActiveCustomer、DeleteActiveUser
 * **/
namespace FxtCollateralManager.Common
{
    [AuthorizeAttribute]
    public class BaseController : Controller
    {
        private string userUrl = "UserUrl";
        private string uploadUrl = "FilePath";
        public object result = null;
        #region wcf调用名称

        #region 贷后 押品管理
        /// <summary>
        /// 押品管理添加临时楼盘
        /// </summary>
        public const string _DATAProjectAdd = "DATAProjectAdd";
        /// <summary>
        /// 押品管理添加临时楼栋
        /// </summary>
        public const string _DATABuildingAdd = "DATABuildingAdd";
        /// <summary>
        /// 押品管理添加临时房号
        /// </summary>
        public const string _DATAHouseAdd = "DATAHouseAdd";
        /// <summary>
        /// 押品管理添加 押品
        /// </summary>
        public const string _DATACollateralAdd = "DataCollateralAdd";
        /// <summary>
        /// 押品管理获得 押品
        /// </summary>
        public const string _DATAGetDataCollateral = "GetDataCollateral";
        /// <summary>
        /// 押品管理修改 押品
        /// </summary>
        public const string _DATADataCollateralUpdate = "DataCollateralUpdate";
        /// <summary>
        /// 押品管理 获取全部信息
        /// </summary>
        public const string _DATAGetAllDataCollateral = "GetAllDataCollateral";
        /// <summary>
        ///  押品管理 获相关某文件的全部押品信息
        /// </summary>
        public const string _DATAGetDataCollateralByFileId = "GetAllDataCollateralByFileId";
        /// <summary>
        /// 押品管理 获得自定义列值
        /// </summary>
        public const string _DATGetCustomColumnsValue = "GetCustomColumnsValue";
        /// <summary>
        /// 管理押品 更新自定义列值
        /// </summary>
        public const string _DATUpdateCustomColumnsValue = "UpdateCustomColumnsValue";
        /// <summary>
        /// 楼盘所有用途
        /// </summary>
        public const string _GetAllProjectPurposeCode = "GetAllProjectPurposeCode";
        /// <summary>
        /// 根据城市ID+楼盘名称 模糊检索出楼盘信息
        /// </summary>
        public const string _GetProjectByCityIDAndLikePrjName = "GetProjectByCityIDAndLikePrjName";
        /// <summary>
        /// 根据城市ID、楼盘ID、楼栋名称得到楼栋
        /// </summary>
        public const string _GetBuildingByProjectIdCityIDAndLikeBuildingName = "GetBuildingByProjectIdCityIDAndLikeBuildingName";
        /// <summary>
        /// 根据城市ID、楼栋ID、房号名称等到房号
        /// </summary>
        public const string _GetHouseByBuildingIdCityIDAndLikeHouseName = "GetHouseByBuildingIdCityIDAndLikeHouseName";
        #endregion
        #region 贷后 押品检测
        /// <summary>
        /// 根据省份或者城市或者行政区得到楼盘总量信息
        /// </summary>
        public const string _CollateralCountByPCA = "GetCollateralCountByPCA";
        /// <summary>
        /// 图形统计
        /// </summary>
        public const string _GetCollateralClassification = "GetCollateralClassification";
        /// <summary>
        /// 押品明细
        /// </summary>
        public const string _GetDetials = "GetDetials";
        /// <summary>
        /// 模糊查找已匹配成功的所有楼盘
        /// </summary>
        public const string _GetProjectByDataCollateral = "GetProjectByDataCollateral";
        /// <summary>
        /// 模糊查找已匹配成功所有开发商
        /// </summary>
        public const string _GetCompanyByDataCollateral = "GetCompanyByDataCollateral";
        /// <summary>
        /// 获得Code列表,根据ID
        /// </summary>
        public const string _GetSYSCodeByID = "GetSYSCodeByID";
        /// <summary>
        /// 获得单个Code对象
        /// </summary>
        public const string _GetSYSCodeByCode = "GetSYSCodeByCode";
        #endregion
        #region 贷后 押品复估
        /// <summary>
        /// 复估管理
        /// </summary>
        public const string _DATAGetCollateralsByReassessment = "GetCollateralsByReassessment";
        /// <summary>
        /// 获得押品的复估值
        /// </summary>
        public const string _DATAGetReassessment = "GetReassessment";
        /// <summary>
        /// 押品复估任务
        /// </summary>
        public const string _DATAAddTask = "AddTask";
        /// <summary>
        /// 更改复估值
        /// </summary>
        public const string _DATAUpdateReassessment = "UpdateReassessment";
        /// <summary>
        /// 复估风险分析
        /// </summary>
        public const string _DATAGetRiskAnalysis = "GetRiskAnalysis";
        /// <summary>
        /// 复估明细查询
        /// </summary>
        public const string _DATAReassessmentDetails = "ReassessmentDetails";

        #endregion
        #region 压力测试
        /// <summary>
        /// 价格走势分析
        /// </summary>
        public const string _GetCollateralPriceTrend = "CollateralPriceTrend";
        /// <summary>
        /// 压力测试
        /// </summary>
        public const string _GetStressTest = "StressTest";
        /// <summary>
        /// 风险预警
        /// </summary>
        public const string _GetRiskWarning = "RiskWarning";
        /// <summary>
        /// 风险预警 危险列表
        /// </summary>
        public const string _GetRikWarningToDanger = "RikWarningToDanger";
        #endregion
        #region 省市县区
        /// <summary>
        /// 获得所有省份
        /// </summary>
        public const string _GetProvince = "GetProvince";
        /// <summary>
        /// 获得所有城市 根据省份ID
        /// </summary>
        public const string _GetCity = "GetCity";
        /// <summary>
        /// 获得行政区 根据城市ID
        /// </summary>
        public const string _GetArea = "GetArea";
        /// <summary>
        /// Api 省 根据ID
        /// </summary>
        public const string _GetProvinceById = "GetProvinceADOById";
        /// <summary>
        /// Api 市 根据城市ID
        /// </summary>
        public const string _GetCityByCityId = "GetCityADOByCityId";
        /// <summary>
        /// Api 县区 根据行政区ID
        /// </summary>
        public const string _GetAreaByAreaId = "GetAreaADOByAreaId";
        #endregion
        #region FxtProject
        /// <summary>
        /// 获得所有银行信息
        /// </summary>
        public const string _GetPriviCompanyAllBank = "GetPriviCompanyAllBank";
        #endregion


        #region 项目及任务
        /// <summary>
        /// 创建修改所属银行项目
        /// </summary>
        public const string _AddEditProjects = "AddEditProjects";
        /// <summary>
        /// 文件项目列表
        /// </summary>
        public const string _GetSysBankProjectList = "GetSysBankProjectList";
        /// <summary>
        /// 项目所有城市
        /// </summary>
        public const string _GetAppointCity = "GetAppointCity";
        /// <summary>
        /// 任务列表
        /// </summary>
        public const string _GetTaskList = "GetTaskList";
        /// <summary>
        /// 任务日志列表
        /// </summary>
        public const string _GetTaskLogList = "GetTaskLogList";
        /// <summary>
        /// 修改任务状态
        /// </summary>
        public const string _EditTaskStatus = "EditTaskStatus";
        /// <summary>
        /// 押品任务导出
        /// </summary>
        public const string _TaskExport = "TaskExport";
        /// <summary>
        /// 导入复估押品
        /// </summary>
        public const string _TaskExcelUp = "TaskExcelUp";
        #endregion
        #region 客户、用户管理
        /// <summary>
        /// 验证客户名称
        /// </summary>
        public const string _CheckCustomerName = "CheckCustomerName";
        /// <summary>
        /// 新增、修改客户
        /// </summary>
        public const string _AddEditCustomer = "AddEditCustomer";
        /// <summary>
        /// 删除、激活客户
        /// </summary>
        public const string _DeleteActiveCustomer = "DeleteActiveCustomer";
        /// <summary>
        /// 客户列表
        /// </summary>
        public const string _GetCustomer = "GetCustomer";
        /// <summary>
        /// 验证用户名称
        /// </summary>
        public const string _CheckUserName = "CheckUserName";
        /// <summary>
        /// 新增、修改用户
        /// </summary>
        public const string _AddEditUser = "AddEditUser";
        /// <summary>
        /// 删除、激活用户
        /// </summary>
        public const string _DeleteActiveUser = "DeleteActiveUser";
        /// <summary>
        /// 修改密码
        /// </summary>
        public const string _ModifyPassword = "ModifyPassword";
        /// <summary>
        /// 用户列表
        /// </summary>
        public const string _GetUser = "GetUser";
        /// <summary>
        /// 验证用户名称
        /// </summary>
        public const string _UserLogin = "UserLogin";

        #region
        public const string _GetUploads = "GetUploads";
        public const string _Uploads = "Uploads";
        #endregion

        #endregion

        #endregion
        /// <summary>
        /// 创建httpClient对象
        /// </summary>
        /// <returns></returns>
        public HttpClient httpClient = new HttpClient();
        /// <summary>
        /// 获得绝对地址
        /// </summary>
        /// <param name="address">相对地址,例如:api/xx/</param>
        /// <returns></returns>
        //public string GetUrl(string address)
        //{
        //    return string.Format("http://localhost:4430/{0}", address);
        //}        
        
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{            
            //base.OnActionExecuting(filterContext);
        //}

        //重写异常处理
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                Exception exception = filterContext.Exception.InnerException;
                JsonResult result = new JsonResult();
                result.Data = new
                {
                    type = "error",
                    message = exception != null ?
                    filterContext.Exception.InnerException.Message :
                    filterContext.Exception.Message
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/index");
            }

            //base.OnException(filterContext);
        }



        /// <summary>
        /// 获得Url信息
        /// </summary>
        /// <param name="key">user地址下标</param>
        /// <returns></returns>
        public string GetUserUrl(string key)
        {
            return string.Format("{0}{1}", GetAppSettingValue(userUrl), GetAppSettingValue(key));
        }

        protected string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #region 上传

        /// <summary>
        /// 获得上传地址
        /// </summary>
        /// <returns></returns>
        public string GetUploadUrl()
        {
            return GetAppSettingValue(uploadUrl);
        }

        public string FileNewName(string strExtension)
        {
            return string.Format("{0}{1}",
                DateTime.Now.ToString("yyyyMMddHHmmssff"),
                strExtension);
        }
        /// <summary>
        /// 获得当前物理路径
        /// </summary>
        /// <param name="file">文件名称</param>
        /// <returns></returns>
        public string GetCurrentPath(string file)
        {
            return System.IO.Path.Combine(GetAppSettingValue(uploadUrl), string.Format("temp\\{0}", file));
        }

        #endregion

        #region 用户

        /// <summary>
        /// 得到系统验证码
        /// </summary>
        /// <returns></returns>
        public string GetSysCode(string strCode)
        {
            //string strdate = DateTime.Now.ToShortDateString();
            return Utils.GetMd5("123" + strCode + "321");
        }

        /// <summary>
        /// 公共参数设置
        /// </summary>
        /// <param name="list">参数集合</param>
        /// <param name="type">操作类型</param>
        public void SetSendType(List<KeyValuePair<string, string>> list,
            string type = null)
        {
            string strdate = DateTime.Now.ToString();
            list.Add(new KeyValuePair<string, string>("strdate", strdate));
            list.Add(new KeyValuePair<string, string>("strcode", GetSysCode(strdate)));
            if (!string.IsNullOrEmpty(type))
                list.Add(new KeyValuePair<string, string>("type", type));
        }

        /// <summary>
        /// 解析关于用户返回Json结果
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public object ResultUserJson(string data)
        {
            JObject jobject = JObject.Parse(data);
            return new
            {
                type = jobject["returntype"] != null ? jobject["returntype"].ToString() : "",
                count = jobject["returntext"] != null ? jobject["returntext"].ToString() : "",
                data = jobject["data"] != null ? jobject["data"].ToString() : ""
            };
        }



        /// <summary>
        /// HttpClient公共参数设置
        /// </summary>
        /// <param name="list">参数集合</param>
        /// <returns></returns>
        public HttpContent GetFormUrlEncodedContent(List<KeyValuePair<string, string>> list)
        {
            return new FormUrlEncodedContent(list);
        }

        #endregion


        public object ResultServerJson(string data, bool flag = true)
        {
            if (!string.IsNullOrEmpty(data))
            {
                JObject jobject = JObject.Parse(data);
                return new
                {
                    type = jobject["type"] != null ? jobject["type"].ToString() : "",
                    message = jobject["message"] != null ? jobject["message"].ToString() : "",
                    data = jobject["data"] != null ? jobject["data"].ToString() : "",
                    count = jobject["count"] != null ? jobject["count"].ToString() : ""
                };
            }
            else
            {
                return new
                {
                    type = flag ? 1 : 0
                };
            }
        }

        /// <summary>
        /// 序列化一个对象为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return Utils.Serialize(obj);
        }

        //获得文件路径
        public string[] getDownFile(string filename = null)
        {
            string filesname = string.Format("导出{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")),
                path = GetCurrentPath("");
            if (!Utils.IsNullOrEmpty(filename))
                filesname = filename;
            return new string[] { 
                path,
                filesname
            };
        }
    }
}
