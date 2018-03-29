using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.Manager.Common;
using FxtSpider.Common;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;
using System.Reflection;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;

namespace FxtSpider.Manager.Web.Controllers
{
    public class ProjectController : BaseController
    {
        #region (AddProject_Fancybox)
        public ActionResult AddProject_Fancybox(string projectName,string cityName,string areaName,long? caseId)
        {
            projectName = JsonHelp.DecodeField(projectName);
            cityName = JsonHelp.DecodeField(cityName);
            areaName = JsonHelp.DecodeField(areaName);
            int nowProvinceId = 0;
            int nowCityId = 0;
            int nowAreaId = 0;
            FxtApi_SYSCity city = CityApi.GetCityByCityName(cityName);
            List<FxtApi_SYSProvince> provinceList = ProvinceApi.GetAllProvince();
            List<FxtApi_SYSCode> purposeCodeList = SysCodeApi.GetAllProjectPurposeCode();
            List<FxtApi_SYSCity> cityList = new List<FxtApi_SYSCity>();
            List<FxtApi_SYSArea> areaList = new List<FxtApi_SYSArea>();
            if (city != null)
            {
                nowProvinceId = city.ProvinceId;
                nowCityId = city.CityId;
                cityList = CityApi.GetCityByProvinceId(nowProvinceId);
                areaList = AreaApi.GetAreaByCityId(nowCityId);
                if(areaList!=null)
                {
                    FxtApi_SYSArea area = areaList.Where(p => !string.IsNullOrEmpty(areaName) && (p.AreaName.Contains(areaName) || areaName.Contains(p.AreaName))).FirstOrDefault();
                    if (area != null)
                    {
                        nowAreaId = area.AreaId;
                    }
                }

            }
            ViewBag.Address = "";
            ViewBag.nowProvinceId = nowProvinceId;
            ViewBag.nowCityId = nowCityId;
            ViewBag.nowAreaId = nowAreaId;
            ViewBag.projectName = projectName;
            ViewBag.provinceList = provinceList;
            ViewBag.purposeCodeList = purposeCodeList;
            ViewBag.cityList = cityList;
            ViewBag.areaList = areaList;
            if (caseId != null)
            {
                案例信息 caseObj = CaseManager.GetCaseById(Convert.ToInt64(caseId));
                if (caseObj != null&&caseObj.地址!=null)
                {
                    ViewBag.Address = caseObj.地址;
                }
            }
            
            return View();
        }
        /// <summary>
        /// 提交新增楼盘
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="purposeCode"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AddProject_FancyboxSubmit_Api(string projectName, int cityId, int areaId, int purposeCode, string address)
        {
            //FxtApi_SYSCode en = new FxtApi_SYSCode();
            //en.Code = 1;
            //en.CodeName = "";
            //en.CodeType = "";
            //en.ID = 11;
            //en.Remark = "";

            string json = "";
            projectName = projectName.DecodeField().TrimBlank();
            address = address.DecodeField();
            if (string.IsNullOrEmpty(projectName))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写楼盘名"));
                Response.End();
                return null;
            }
            if (cityId < 1||areaId<1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请选择城市和行政区"));
                Response.End();
                return null;
            }
            if (purposeCode < 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请选择用途"));
                Response.End();
                return null;
            }
            string message = "";
            bool result = ProjectApi.InsertProjectApi(projectName, cityId, areaId, purposeCode, address, out message);
            json = WebJsonHelp.MvcResponseJson("", result: result ? 1 : 0, message: message);
            Response.Write(json);
            Response.End();
            return null;
        }

        
        #endregion
    }
}
