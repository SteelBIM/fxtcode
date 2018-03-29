using CAS.Common;
using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.FxtLoanDomain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

/**
 *      修改人:贺黎亮  2014.06.11 
 *             1.添加方法GetSysBankProjectList,AddEditProjects
 * **/
namespace FxtCollateralManager.Web.Controllers
{

    public class ProjectsController : BaseController
    {
       
        public ActionResult Index()
        {
            return View();
        }

        //获得银行列表
        public ActionResult GetBankCompany() {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                  "D", _GetPriviCompanyAllBank, "");
                return Json(result);
            }
        }

        //新增修改文件项目
        public ActionResult AddEditProjects(SysBankProject data,int delvalid=1)
        {
            JObject _obj = new JObject();
            data.CreateDate = DateTime.Now;
            data.CustomerId = Public.LoginInfo.CustomerId;
            data.Valid = delvalid;
            data.UserId = Public.LoginInfo.Id;
            _obj.Add("data", Utils.Serialize(data));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _AddEditProjects, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //获取文件项目列表
        public ActionResult GetSysBankProjectList(int pageIndex = 0, int pageSize = 0
            , int id = 0, string projectname = "", int bankid = 0)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (Public.LoginInfo.CustomerType == (int)Common.EnumHelper.CustomerType.Company_Bank)
                {
                    bankid = Public.LoginInfo.CustomerId;
                }
                JObject obj = new JObject();
                obj.Add("pageIndex", pageIndex);
                obj.Add("pageSize", pageSize);
                obj.Add("id", id);
                obj.Add("key", projectname);
                obj.Add("bankid", bankid);
                obj.Add("orderProperty", "  id desc ");
                obj.Add("customerid", Public.LoginInfo.CustomerId);
                obj.Add("customertype", Public.LoginInfo.CustomerType);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetSysBankProjectList, Utils.Serialize(obj)).ToString());
            }
        }


        
        //获取文件项目列表
        public ActionResult GetAppointCity()
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("customerId", Public.LoginInfo.CustomerId);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetAppointCity, Utils.Serialize(obj)).ToString().ToLower());
            }
        }
        
    }
}
